using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HittableWBar : TrashHittable
{
    [SerializeField] protected UiBar bar = null;
    [SerializeField] float timeToDissappear = 1;
    [SerializeField] protected float speedToDecrease = 2;

    public bool barAuth;

    protected bool barDowngrade = true;

    protected override void Start()
    {
        base.Start();
        bar = Character.instance.myBar;
    }

    protected virtual void Update()
    {
        if (!barAuth) return;
        if (barDowngrade && !bar.IsEmpty)
        {
            bool b = bar.RestToBar(Time.deltaTime * speedToDecrease);

            if (b) bar.ShutDown();
        }
    }
}
