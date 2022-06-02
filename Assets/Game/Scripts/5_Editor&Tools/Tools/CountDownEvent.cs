using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;
using System;

public class CountDownEvent : CountDownMonovehaviour
{
    [SerializeField] UnityEvent UE_OnBegin;
    [SerializeField] UnityEvent UE_OnFinish;
    [SerializeField] EventFloat UE_CDRemain;

    public override void Play()
    {
        base.Play();
        UE_OnBegin.Invoke();
    }

    public override void Play(Action _OnFinish, float _timer, bool _forward)
    {
        base.Play(_OnFinish, _timer, _forward);
        UE_OnBegin.Invoke();
    }

    protected override void OnFinish()
    {
        base.OnFinish();
        UE_OnFinish.Invoke();
    }

    protected override void TICK_Remain(float _time)
    {
        base.TICK_Remain(_time);
        UE_CDRemain.Invoke(_time);
    }
}
