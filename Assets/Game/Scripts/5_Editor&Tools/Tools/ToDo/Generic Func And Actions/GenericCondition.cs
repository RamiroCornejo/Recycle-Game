using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericCondition
{
    public abstract bool Condition();
}

public class IsTargetNearCondition : GenericCondition
{
    float distanceToNear;
    Transform target;
    Transform myTransform;

    public IsTargetNearCondition(float distance, Transform _target, Transform _myTransform)
    {
        distanceToNear = distance;
        target = _target;
        myTransform = _myTransform;
    }

    public override bool Condition()
    {
        float distance = (target.position - myTransform.position).sqrMagnitude;
        return distance * distance <= distanceToNear;
    }
}
