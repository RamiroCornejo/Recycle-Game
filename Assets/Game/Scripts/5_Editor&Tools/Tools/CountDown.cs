using UnityEngine;
using System;

public class CountDown
{
    Action callback_onFinish = delegate { };
    float maxtime = 1.0f;
    bool isInCD = false;
    float current_time;
    public bool InCD => isInCD;
    bool alive = false;
    public float Remain => current_time;
    bool forward = false;

    public CountDown()
    {
        alive = false;
        isInCD = false;

        callback_onFinish = delegate { };
        maxtime = 1.0f;
        this.forward = false;
    }
    public CountDown(Action onFinish, float time = 1.0f, bool forward = false)
    {
        alive = false;
        isInCD = false;

        callback_onFinish = onFinish;
        maxtime = time;
        this.forward = forward;
    }

    public void Start()
    {
        alive = true;
        isInCD = true;
        if (forward) current_time = 0;
        else current_time = maxtime;
    }
    public void Start(Action onFinish = null, float time = 1.0f, bool forward = false)
    {
        if(onFinish != null) callback_onFinish = onFinish;
        maxtime = time;
        this.forward = forward;
        alive = true;
        isInCD = true;
        if (forward) current_time = 0;
        else current_time = maxtime;
    }

    public void Pause() => alive = false;
    public void Resume() => alive = true;
    public void Reset() => Start();
    public void Stop()
    {
        alive = false;
        isInCD = false;
        if (forward) current_time = 0;
        else current_time = maxtime;
    }

    public void Tick(float DeltaTime)
    {
        if (!alive) return;

        if (isInCD)
        {
            if (forward ? current_time < maxtime : current_time > 0)
            {
                current_time = current_time + (forward ? 1 : -1) * DeltaTime;
            }
            else
            {
                isInCD = false;
                current_time = forward ? 0 : maxtime;
                callback_onFinish?.Invoke();
            }
        }
    }
}
