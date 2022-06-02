using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AnimEvent))]
public class MachineAnimations : MonoBehaviour
{
    AnimEvent myAnimEvent;
    Animator myAnimator;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myAnimEvent = GetComponent<AnimEvent>();
    }

    public void SubscribeToSpitEvent(Action OnSpit)
    {
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("Spit", OnSpit);
    }

    public void PlayProcess()
    {
        myAnimator.SetBool("idle", true);
    }
    public void StopProcess()
    {
        myAnimator.SetBool("idle", false);
    }
    public void SpitAnim()
    {
        myAnimator.Play("Spit");
    }
}
