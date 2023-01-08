using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRootMotionParentConstraint : MonoBehaviour
{
    Transform dummyTrans;
    [SerializeField] Transform Source;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dummy = new GameObject("postRootMotionConstraintDummy");
        dummy.transform.position = transform.position;
        dummy.transform.rotation = transform.rotation;
        dummy.transform.SetParent(Source,true);
        dummyTrans = dummy.transform;

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = dummyTrans.position;
        //transform.rotation = dummyTrans.rotation;
    }

    private void LateUpdate()
    {

        transform.position = dummyTrans.position;
        transform.rotation = dummyTrans.rotation;
    }
}
