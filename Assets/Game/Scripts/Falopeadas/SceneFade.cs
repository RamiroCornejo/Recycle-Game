using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneFade : MonoBehaviour
{
    bool anim;
    float timer;
    int dir;
    [SerializeField] float speed = 1;
    [SerializeField] MeshRenderer[] mats = new MeshRenderer[0];
    [SerializeField] GameObject reverseMats = null;
    [SerializeField] BendingManager managerBend = null;
    [SerializeField] UnityEvent SceneFadeOver = new UnityEvent();
    [SerializeField] UnityEvent SceneFadeOut = new UnityEvent();
    [SerializeField] GameObject godRay = null;

    [SerializeField] bool startWithFade = false;

    private void Start()
    {
        if (startWithFade)
        {
            reverseMats.SetActive(false);
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].gameObject.SetActive(true);
                mats[i].material.SetFloat("_Transition", 1);
            }
            godRay.SetActive(true);
            timer = 1;
            dir = 1;
        }
    }


    public void TerrainFadeIn()
    {
        dir = 1;
        anim = true;
        reverseMats.SetActive(false);
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].gameObject.SetActive(true);
        }
        //managerBend.ModifyBend(0);
    }

    public void TerrainFadeOut()
    {
        dir = -1;
        anim = true;
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].gameObject.SetActive(true);
        }
        godRay.SetActive(false);
        //managerBend.ModifyBend(0.006f);
        reverseMats.SetActive(true);
    }


    private void Update()
    {
        if (!anim) return;

        timer += Time.deltaTime * dir * speed;

        timer = Mathf.Clamp(timer, 0, 1);
        float reverseOpacity = 1 - timer;
        for (int i = 0; i < mats.Length; i++)
        {
           // Debug.Log(timer);
            mats[i].material.SetFloat("_Transition", timer);
        }



        if (timer <= 0 || timer >= 1)
        {
            if (timer < 0.5f)
            {
                SceneFadeOver.Invoke();
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].gameObject.SetActive(false);
                }
            }
            else { SceneFadeOut.Invoke(); godRay.SetActive(true); }


            anim = false;
        }
    }
}
