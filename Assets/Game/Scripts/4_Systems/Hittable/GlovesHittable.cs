using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovesHittable : HittableWBar
{
    [SerializeField] float speedToIncrease = 1;

    protected override void OnExecuteErrorMesseage()
    {
        Debug.Log("No tenés gloves padre");
    }

    protected override void OnHit()
    {
        barDowngrade = false;
        bar.ShutOn(this);
    }

    protected override void OnReleaseHit()
    {
        barDowngrade = true;
        bar.ShutDown();
    }

    public override void OnResetHittable()
    {
        barDowngrade = true;
        bar.ShutDown();
    }

    protected override void Update()
    {
        base.Update();
        if (!barDowngrade)
        {
            bool b = bar.AddToBar(Time.deltaTime * speedToIncrease);

            if (b)
            {
                bar.ShutDown();
                Character.instance.ReleaseAttack();
                Dead();
            }
        }
    }
}
