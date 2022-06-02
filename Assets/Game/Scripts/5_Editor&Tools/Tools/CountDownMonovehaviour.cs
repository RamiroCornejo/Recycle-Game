using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountDownMonovehaviour : MonoBehaviour
{
    [SerializeField] string myname = "";
    [SerializeField] float timer;
    [SerializeField] bool forward = false;
    [SerializeField] bool begin_on_start;
    CountDown myCountDown = new CountDown();

    private void Start()
    {
        myCountDown = new CountDown(OnFinish, timer, forward);
        if (begin_on_start)
        {
            myCountDown.Start();
        }
    }

    public virtual void Play()
    {
        myCountDown.Start(OnFinish, timer, forward);
    }
    public virtual void Play(Action _OnFinish, float _timer, bool _forward)
    {
        myCountDown.Start(() => { _OnFinish.Invoke(); OnFinish(); }, _timer, _forward);
    }

    protected virtual void OnFinish()
    {
        //overridear
    }
    protected virtual void TICK_Remain(float _time)
    {
        //overridear
    }
    private void Update()
    {
        myCountDown.Tick(Time.deltaTime);
        if (myCountDown.InCD)
        {
            TICK_Remain(myCountDown.Remain);
        }
    }

    public override string ToString()
    {
        string currentname = myname == "" ? gameObject.name : myname;
        return "CD[" + myname + "]=" + myCountDown.Remain.ToString();
    }
}
