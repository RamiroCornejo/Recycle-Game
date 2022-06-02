using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PingPongLerp
{
    float timer = 0f;
    bool tick_alive = false;
    bool go = true;
    bool invert = false;
    bool loop = true;
    byte maxCounter;
    float time_to_flip = 1f;
    float time_to_flop = 1f;
    byte counter;
    float cd_timer;
    float cd = 1;
    bool hasCd;
    bool isOnCd = false;
    //bool ignore_lerp = false;

    Action<float> callback_lerp = delegate { };
    Action flip = delegate { };
    Action flop = delegate { };

    public PingPongLerp Configure_Callback(Action<float> callback_lerp)
    {
        this.callback_lerp = callback_lerp;
        return this;
    }

    public PingPongLerp Configure_TEMPLATE_LOOP()
    {
        loop = true;
        return this;
    }

    public PingPongLerp Configure_TEMPLATE_LOOP_CD_TIMER(float _timer)
    {
        loop = true;
        hasCd = true;
        cd = _timer;
        return this;
    }

    public PingPongLerp Configure_TEMPLATE_ONLY_FLIP(float time_to_flip = 1)
    {
        loop = true;
        this.time_to_flip = time_to_flip;
        this.time_to_flop = 0;
        return this;
    }
    public PingPongLerp Configure_TEMPLATE_ONLY_FLOP(float time_to_flop = 1)
    {
        loop = true;
        this.time_to_flip = 0;
        this.time_to_flop = time_to_flop;
        return this;
    }

    public PingPongLerp Configure_TEMPLATE_ONESHOT()
    {
        loop = false;
        maxCounter = 2;
        return this;
    }
    public PingPongLerp Configure_TEMPLATE_ONESHOT_EXPLOSION(float time_to_flip = 0.1f, float time_to_flop = 1)
    {
        loop = false;
        maxCounter = 2;
        invert = true;
        this.time_to_flip = time_to_flip;
        this.time_to_flop = time_to_flop;
        return this;
    }
    public PingPongLerp Configure_TEMPLATE_LOOP_EXPLOSION(float time_to_flip = 0.1f, float time_to_flop = 1)
    {
        loop = true;
        maxCounter = byte.MaxValue;
        invert = true;
        this.time_to_flip = time_to_flip;
        this.time_to_flop = time_to_flop;
        return this;
    }
    public PingPongLerp Configure_TEMPLATE_FLIP_FLOP(float timer)
    {
        loop = true;
        time_to_flip = timer;
        time_to_flop = timer;
        //ignore_lerp = true;
        return this;
    }

    /// <summary>
    /// Initial configuartion
    /// </summary>
    /// <param name="callback_lerp"> Un Action que por parametro recibe valores flotantes entre 0 y 1 </param>
    /// <param name="invert"> [ TRUE va desde 0 a 1 ]  :::: [ FALSE va desde 1 a 0 ] </param>
    /// <param name="time_to_flip"> el tiempo que se usa solo para llegar al final </param>
    /// <param name="time_to_flop"> el tiempo que se usa solo para volver al inicio </param>
    public void Configure(Action<float> callback_lerp, bool loop = true, byte maxCounter = 2, bool invert = false, float time_to_flip = 1.0f, float time_to_flop = 1.0f, bool hasCd = false, float cd_timer = 0f)
    {
        this.callback_lerp = callback_lerp;
        this.loop = loop;
        this.maxCounter = maxCounter;
        this.invert = invert;
        this.time_to_flip = time_to_flip;
        this.time_to_flop = time_to_flop;
        this.hasCd = hasCd;
        this.cd_timer = cd_timer;
    }

    public PingPongLerp ConfigureCallbacksAuxiliars(Action flip, Action flop)
    {
        this.flip = flip;
        this.flop = flop;
        return this;
    }

    public PingPongLerp Play()
    {
        tick_alive = true;
        go = !invert;

        timer = go ? 0 : time_to_flop;

        if (!loop)
        {
            counter = 0;
        }

        return this;
    }
    public void Stop()
    {
        tick_alive = false;
        counter = 0;
        go = invert;
    }

    public void Tick(float DeltaTime)
    {
        if (tick_alive)
        {
            if (!isOnCd)
            {
                if (go)
                {
                    if (timer < time_to_flip)
                    {
                        Debug.Log("GO");
                        timer = timer + 1 * Time.deltaTime;
                        /*if(!ignore_lerp)*/ callback_lerp.Invoke(timer / time_to_flip);
                    }
                    else
                    {
                        flip.Invoke();
                        timer = time_to_flop;
                        go = false;
                        CheckLoopCounter();
                        isOnCd = hasCd;
                    }
                }
                else
                {
                    if (timer > 0)
                    {
                        Debug.Log("BACK");
                        //Debug.Log("timer: " + timer);
                        timer = timer - 1 * Time.deltaTime;
                        /*if (!ignore_lerp)*/ callback_lerp.Invoke(timer / time_to_flop);
                    }
                    else
                    {
                        flop.Invoke();
                        timer = 0;
                        go = true;
                        CheckLoopCounter();
                        isOnCd = hasCd;
                    }
                }
            }
            else
            {
                Debug.Log("ONCD");
                if (cd_timer < cd)
                {
                    cd_timer = cd_timer + 1 * Time.deltaTime;
                }
                else
                {
                    cd_timer = 0;
                    isOnCd = false;
                }
            }
        }
    }

    void CheckLoopCounter()
    {
        if (!loop)
        {
            counter++;
            if (counter >= maxCounter)
            {
                Stop();
            }
        }
    }
}
