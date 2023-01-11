using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIKWeightController : MonoBehaviour
{
    [SerializeField] GameObject observeObject;
    [SerializeField] float transition = 1f;
    [SerializeField] RootMotion.FinalIK.AimIK aimIK;
    [SerializeField] bool invert;
    RootMotion.FinalIK.IKSolverAim solverAim;
    int current = 0;
    int target = 0;
    float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        solverAim = aimIK.solver;    
    }

    // Update is called once per frame
    void Update()
    {
        target = 0;
        if (observeObject.activeInHierarchy)
        {
            target = 1;
        }
        progress += Time.deltaTime * (1 / transition) * (target - current);
        if (progress <= 0f)
        {
            current = 0;
        }
        else if (progress >= 1f)
        {
            current = 1;
        }
        solverAim.SetIKPositionWeight(invert ? 1 - progress : progress);
    }
}
