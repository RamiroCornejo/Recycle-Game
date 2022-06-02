using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerComponent_GroundCheck : MonoBehaviour
{
    [SerializeField] bool UseFixedUpdate = false;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] bool isGrounded;

    public bool hasWindow = true;
    public float window_time = 0.1f;
    bool anim;
    float timer_window;

    public bool IsGrounded { get => isGrounded; }

    bool oneShotHotRes;

    private void Update()
    {
        if (!UseFixedUpdate)
        {
            bool hotRes = Physics.CheckSphere(this.transform.position, groundDistance, groundMask);

            if (hotRes)
            {
                isGrounded = true;
            }
            else
            {
                if (!oneShotHotRes)
                {
                    oneShotHotRes = true;
                    anim = true;
                }
            }

            if (anim)
            {
                if (timer_window < window_time)
                {
                    timer_window = timer_window + 1 * Time.deltaTime;
                }
                else
                {
                    timer_window = 0;
                    anim = false;
                    oneShotHotRes = false;
                    isGrounded = false;
                }
            }

           // isGrounded = Physics.CheckSphere(this.transform.position, groundDistance, groundMask);
        }
    }
    private void FixedUpdate()
    {
        if (UseFixedUpdate)
        {
            bool hotRes = Physics.CheckSphere(this.transform.position, groundDistance, groundMask);

            if (hotRes)
            {
                isGrounded = true;
            }
            else
            {
                if (!oneShotHotRes)
                {
                    oneShotHotRes = true;
                    anim = true;
                }
            }

            if (anim)
            {
                if (timer_window < window_time)
                {
                    timer_window = timer_window + 1 * Time.deltaTime;
                }
                else
                {
                    timer_window = 0;
                    anim = false;
                    oneShotHotRes = false;
                    isGrounded = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, groundDistance);
    }
}