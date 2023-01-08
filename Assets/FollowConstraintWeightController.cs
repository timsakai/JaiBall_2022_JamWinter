using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FollowConstraintWeightController : MonoBehaviour
{
    [SerializeField] GameObject observeObject;
    [SerializeField] float transition = 1f;
    [SerializeField] GameObject constraintObject;
    IConstraint constraint;
    [SerializeField] int onDisableIndex = 0;
    [SerializeField] int onEnableIndex = 1;
    int current = 0;
    int target = 0;
    float progress = 0f;
    // Start is called before the first frame update
    void Start()
    {
        constraint = constraintObject.GetComponent<IConstraint>();
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
        else if(progress >= 1f)
        {
            current = 1;
        }
        ConstraintSource source0 = constraint.GetSource(onDisableIndex);
        source0.weight = 1 - progress;
        constraint.SetSource(onDisableIndex, source0);
        ConstraintSource source1 = constraint.GetSource(onEnableIndex);
        source1.weight = progress;
        constraint.SetSource(onEnableIndex, source1);
    }
}
