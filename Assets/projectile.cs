using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectile : MonoBehaviour
{
    Rigidbody m_rigidbody;
    public float lifetimeBase = 3.0f;
    float lifetime = 0;
    public float speed = 100;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > lifetimeBase)
        {
            Destroy(gameObject);
        }
    }

    public void Throw()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddForce(transform.forward * speed,ForceMode.VelocityChange);
    }
}
