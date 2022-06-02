using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Curtain : MonoBehaviour
{
    public static Curtain instance;
    [SerializeField] float maxtime;
    float timer = 0;
    bool go = false;
    bool anim = false;
    [SerializeField] CanvasGroup group = null;
    [SerializeField] Image img = null;
     
    Action callback;

    private void Awake() => instance = this;
    private void Start() { } 
    public static void Open(Action callback) => instance.OpenCurtain(callback);
    public static void Close(Action callback) => instance.CloseCurtain(callback);
    public static void Open() => instance.OpenCurtain(() => { });
    public static void Close() => instance.CloseCurtain(() => { });

    void OpenCurtain(Action callback)
    {
        anim = true;
        go = true;
        this.callback = callback;
    }
    void CloseCurtain(Action callback)
    {
        anim = true;
        go = false;
        this.callback = callback;
    }

    public void AnimLerp(float val)
    {
        if(group) group.alpha = Mathf.Lerp(go ? 0 : 1, go ? 1 : 0, val);
        if(img) img.fillAmount = Mathf.Lerp(go ? 0 : 1, go ? 1 : 0, val);
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < maxtime)
            {
                timer = timer + 1 * Time.deltaTime;
                AnimLerp(timer / maxtime);
            }
            else
            {
                timer = 0;
                anim = false;
                callback.Invoke();
                callback = delegate { };
            }
        }
    }
}
