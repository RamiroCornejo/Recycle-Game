using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMesseage : MonoBehaviour
{
    [SerializeField] float timeActive = 3;
    float timer;
    bool anim;
    bool returning;
    float lerp;
    [SerializeField] float lerpSpeed = 3;

    [SerializeField] TextMeshProUGUI textPro = null;

    Vector3 initialPos;
    Vector3 destinyPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        destinyPos = transform.position - Vector3.up * 400;
        transform.position = destinyPos;
    }

    void Update()
    {
        if (anim)
        {
            if (!returning)
            {
                lerp += Time.deltaTime * lerpSpeed;

                transform.position = Vector3.Lerp(destinyPos, initialPos, lerp);

                if (lerp >= 1)
                {
                    lerp = 0;
                    returning = true;
                }
                return;
            }

            timer += Time.deltaTime;

            if (timer < timeActive) return;

            lerp -= Time.deltaTime * lerpSpeed;

            transform.position = Vector3.Lerp(destinyPos, initialPos, lerp);

            if (lerp <= 1)
            {
                lerp = 0;
                returning = false;
                anim = false;
            }
        }
    }

    public void SendMesseage(string messeage)
    {
        lerp = 0;
        timer = 0;
        anim = true;
        returning = false;
        transform.position = destinyPos;
        textPro.text = messeage;
    }
}
