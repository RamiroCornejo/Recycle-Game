using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    Vector3 InitialPos;
    public Vector3 offsetA;
    public Vector3 offsetB;

    public AnimationCurve go_curve;
    public AnimationCurve back_curve;

    float time_to_go;
    float time_to_back;
    float timer;

    bool go;

    private void Start()
    {
        InitialPos = transform.position;
        offsetA = transform.position + offsetA;
        offsetB = transform.position + offsetB;

        time_to_go = go_curve.keys[go_curve.length - 1].time;
        time_to_back = back_curve.keys[back_curve.length - 1].time;
    }

    void Anim(float lerp)
    {
        if (go)
        {
            this.transform.position = Vector3.Lerp(offsetA, offsetB, lerp/time_to_go);
        }
        else
        {
            this.transform.position = Vector3.Lerp(offsetB, offsetA, lerp/ time_to_back);
        }
    }

    private void Update()
    {
        if (go)
        {
            if (timer < time_to_go)
            {
                timer = timer + 1 * Time.deltaTime;
                Anim(go_curve.Evaluate(timer));
            }
            else
            {
                go = false;
                timer = 0;
            }
        }
        else
        {
            if (timer < time_to_back)
            {
                timer = timer + 1 * Time.deltaTime;
                Anim(back_curve.Evaluate(timer));
            }
            else
            {
                go = true;
                timer = 0;
            }
        }
    }
}
