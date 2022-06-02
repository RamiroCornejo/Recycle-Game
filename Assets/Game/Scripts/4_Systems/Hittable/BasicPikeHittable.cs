using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPikeHittable : TrashHittable
{
    protected override void OnExecuteErrorMesseage()
    {
         
    }

    protected override void OnHit()
    {
        Dead();
    }

    protected override void OnReleaseHit()
    {
    }

    public override void OnResetHittable()
    {

    }
}
