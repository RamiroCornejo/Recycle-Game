using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelHittable : HittableWBar
{
    [SerializeField] float unitsToIncrease = 1.5f;
    [SerializeField] ParticleSystem shovelSand = null;

    protected override void Start()
    {
        base.Start();
        ParticlesManager.Instance.GetParticlePool(shovelSand.name, shovelSand);
    }

    protected override void OnExecuteErrorMesseage()
    {
    }

    protected override void OnHit()
    {
        if (barDowngrade)
        {
            barDowngrade = false;
            bar.ShutOn(this);
        }
        bool b = bar.AddToBar(unitsToIncrease);
        ParticlesManager.Instance.PlayParticle(shovelSand.name, transform.position + Vector3.up);

        if (b)
        {
            Dead();
            bar.ShutDown();
        }
    }

    protected override void OnReleaseHit()
    {
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
            bool b = bar.RestToBar(Time.deltaTime * speedToDecrease);

            if (b)
            {
                bar.ShutDown();
                barDowngrade = true;
            }
        }
    }
}
