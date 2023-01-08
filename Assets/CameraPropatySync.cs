using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPropatySync : MonoBehaviour
{
    [SerializeField] Camera source;
    [SerializeField] bool SyncFOV = true;
    Camera self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SyncFOV) self.fieldOfView = source.fieldOfView;
    }
}
