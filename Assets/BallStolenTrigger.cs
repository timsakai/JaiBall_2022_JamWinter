using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStolenTrigger : MonoBehaviour
{
    public TPSController tpsController { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        tpsController = GetComponentInParent<TPSController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
