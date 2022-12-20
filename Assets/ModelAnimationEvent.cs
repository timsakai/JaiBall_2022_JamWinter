using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModelAnimationEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnJump;
    [SerializeField] UnityEvent OnShot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        OnJump.Invoke();
    }

    public void Shot()
    {
        OnShot.Invoke();
    }
}
