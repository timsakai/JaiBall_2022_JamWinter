using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BallController : MonoBehaviour
{
    ParentConstraint m_parentConstraint;
    Rigidbody m_rigidbody;
    [SerializeField] float returnTime = 3f;
    [SerializeField] List<TPSController> haveables;
    TPSController current;
    TPSController last;
    [SerializeField] Collider collectTrigger;
    float throwTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_parentConstraint = GetComponent<ParentConstraint>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        int haved = 0;
        foreach (var item in haveables)
        {
            if (item.hasBall)
            {
                haved++;
                if (current != item)
                {
                    if (last == item)
                    {
                        if (throwTimer >= 0.5f)
                        {
                            returnToHandler(item);
                        }
                    }
                    else
                    {
                        returnToHandler(item);
                    }
                }
                current = item;
                last = item;
            }
        }
        if (haved == 0)
        {
            collectTrigger.enabled = true;
            current = null;
        }
        else
        {
            collectTrigger.enabled = false;
        }

        if (throwTimer >= 0)
        {
            throwTimer += Time.deltaTime;
        }
        if (throwTimer >= returnTime)
        {
            throwTimer = -1;
            //returnToPlayer();
        }
    }

    void returnToHandler(TPSController controller)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = controller.ballHoldSocket;
        source.weight = 1;
        m_parentConstraint.SetSource(0, source);
        m_parentConstraint.SetTranslationOffset(0, Vector3.zero);
        m_parentConstraint.constraintActive = true;
        m_rigidbody.isKinematic = true;
    }

    public void Throw(Quaternion rotation)
    {
        throwTimer = 0;
        m_parentConstraint.constraintActive = false;
        m_rigidbody.isKinematic = false;
        m_rigidbody.rotation = rotation;
        m_rigidbody.AddRelativeForce(Vector3.forward * 10, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + " isTriggerEnter on " + other.name);
        BallStealTrigger steal = other.GetComponent<BallStealTrigger>();
        if (steal != null)
        {
            steal.CollectBall();
        }
    }
}
