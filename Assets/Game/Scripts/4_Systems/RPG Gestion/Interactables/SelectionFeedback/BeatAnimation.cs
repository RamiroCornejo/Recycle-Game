using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// >>>>>>>>>>  R E C O R D A R  >>>>>>>>>>
// recordar que cuando lo tiremos al editor...
// la curva tiene que arrancar fuerte y luego 
// ir soltando de a poquito, luego ponerse en 
// el mismo lugar donde arranco
// simulando el latido

public class BeatAnimation : MonoBehaviour
{
    float timer = 0f;
    float time_to_go = 0f;
    public AnimationCurve animcurve;
    bool anim;

    public SpriteRenderer render;
    public Transform target;
    Vector3 original_scale;
    Vector3 offset_scale;
    public float quantity_scale = 0.2f;

    private void Start()
    {
        time_to_go = animcurve.keys[animcurve.length - 1].time;
        original_scale = target.localScale;
        offset_scale = new Vector3(original_scale.x + quantity_scale, original_scale.y + quantity_scale, original_scale.z + quantity_scale);
    }

    void Lerp(float val)
    {
        target.transform.localScale = Vector3.Lerp(original_scale, offset_scale, val);
        render.color = Color.Lerp(new Color(1,1,1,0), Color.white, val);
        //aca hago lo que tengo que hacer
    }

    public void BeginBeatAnimation()
    {
        anim = true;
        timer = 0;
    }
    public void StopBeatAnimation()
    {
        anim = false;
        timer = 0;
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_go)
            {
                timer = timer + 1 * Time.deltaTime;
                Lerp(animcurve.Evaluate(timer));
            }
            else
            {
                timer = 0;
            }
        }
    }
}
