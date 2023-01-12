using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStealTrigger : MonoBehaviour
{
    TPSController tpsController;
    [SerializeField] float collectCoolTime = 1.0f;
    Collider trigger;
    Collider laststolen;
    [SerializeField] float collectTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        tpsController = GetComponentInParent<TPSController>();
        trigger = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectBall()
    {
        tpsController.StealBall();
    }

    private void OnTriggerEnter(Collider other)
    {
        BallStolenTrigger stolen = other.GetComponent<BallStolenTrigger>();
        if (stolen != null)
        {
            tpsController.StealBall();
            stolen.tpsController.StolenBall();
        }
    }
}
