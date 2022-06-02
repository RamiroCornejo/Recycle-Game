using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] float maxHeight = 22;
    [SerializeField] Rigidbody[] characters;

    [SerializeField] float timeToFall = 3;
    float timer;
    bool anim;

    int current = 0;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].velocity = Vector3.zero;
            characters[i].constraints = RigidbodyConstraints.FreezeAll;
            characters[i].transform.position = characters[i].transform.position + Vector3.up * maxHeight;
        }
    }
    void Update()
    {
        if (anim)
        {
            timer += Time.deltaTime;

            if(timer >= timeToFall)
            {
                timer = 0;
                characters[current].constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ & RigidbodyConstraints.FreezeRotation;
                current += 1;
                if (current >= characters.Length)
                    anim = false;
            }
        }
    }


    public void StartCredits()
    {
        timer = timeToFall;
        current = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].velocity = Vector3.zero;
            characters[i].constraints = RigidbodyConstraints.FreezeAll;
            characters[i].transform.position = characters[i].transform.position + Vector3.up * maxHeight;
        }

        anim = true;
    }
}
