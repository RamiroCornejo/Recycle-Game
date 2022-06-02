using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIFade : MonoBehaviour
{
    [SerializeField] CanvasGroup fade = null;
    [SerializeField] float fadeSpeed = 3;
    Action FadeOutOver;
    Action FadeInOver;
    float timer;
    bool anim;
    float dir;

    public void FadeOut(Action _FadeOutOver)
    {
        timer = 1;
        fade.alpha = timer;
        fade.blocksRaycasts = false;
        fade.interactable = false;
        dir = -1;
        FadeOutOver = _FadeOutOver;
        anim = true;
    }

    public void FadeIn(Action _FadeInOver)
    {
        timer = 0;
        fade.alpha = timer;
        fade.blocksRaycasts = true;
        fade.interactable = true;
        dir = 1;
        FadeInOver = _FadeInOver;
        anim = true;
    }

    private void Update()
    {
        if (anim)
        {
            timer += Time.deltaTime * dir * fadeSpeed;

            fade.alpha = timer;

            if(timer >= 1 || timer <= 0)
            {
                anim = false;
                timer = 0;
                if (dir == 1) FadeInOver?.Invoke();
                else FadeOutOver?.Invoke();
            }
        }
    }
}
