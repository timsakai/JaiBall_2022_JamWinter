using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BallController : MonoBehaviour
{
    ParentConstraint m_parentConstraint;
    Rigidbody m_rigidbody;
    [SerializeField] float returnTime = 3f;
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
        if (throwTimer >= 0)
        {
            throwTimer += Time.deltaTime;
        }
        if (throwTimer >= returnTime)
        {
            throwTimer = -1;
            m_parentConstraint.SetTranslationOffset(0, Vector3.zero);
            m_parentConstraint.constraintActive = true;
            m_rigidbody.isKinematic = true;
        }
    }

    public void Throw(Quaternion rotation)
    {
        throwTimer = 0;
        m_parentConstraint.constraintActive = false;
        m_rigidbody.isKinematic = false;
        m_rigidbody.rotation = rotation;
        m_rigidbody.AddRelativeForce(Vector3.forward * 30, ForceMode.VelocityChange);
    }
}
