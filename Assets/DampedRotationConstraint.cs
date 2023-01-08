using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampedRotationConstraint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform source;
    Vector3 euler = new Vector3();
    Vector3 eulerVelocity = new Vector3();
    [SerializeField] float time = 0.5f;
    [SerializeField] bool freezeX;
    [SerializeField] bool freezeY;
    [SerializeField] bool freezeZ;
    void Start()
    {
        euler.x = transform.eulerAngles.x;
        euler.y = transform.eulerAngles.y;
        euler.z = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(freezeX) euler.x = Mathf.SmoothDampAngle(transform.eulerAngles.x, source.eulerAngles.x, ref eulerVelocity.x, time);
        if(freezeY) euler.y = Mathf.SmoothDampAngle(transform.eulerAngles.y, source.eulerAngles.y, ref eulerVelocity.y, time);
        if(freezeZ) euler.z = Mathf.SmoothDampAngle(transform.eulerAngles.z, source.eulerAngles.z, ref eulerVelocity.z, time);
        transform.rotation = Quaternion.Euler(freezeX ? euler.x : transform.eulerAngles.x
                                            , freezeY ? euler.y : transform.eulerAngles.y
                                            , freezeZ ? euler.z : transform.eulerAngles.z);
    }
}
