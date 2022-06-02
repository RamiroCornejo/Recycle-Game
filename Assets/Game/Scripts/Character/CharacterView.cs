using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] Transform root = null;

    [SerializeField] AnimEvent animevents;

    private void Start()
    {
        animevents.ADD_ANIM_EVENT_LISTENER("HitPicker", EventFRAME_HitPicker);
        animevents.ADD_ANIM_EVENT_LISTENER("TakeGloves", EventFRAME_Gloves);
        animevents.ADD_ANIM_EVENT_LISTENER("HitShowel", EventFRAME_HitShowel);
        animevents.ADD_ANIM_EVENT_LISTENER("Step", EventFRAME_Step);
    }

    public bool Flipped { get; private set; }

    public void Flip(float x, float y)
    {
        if (!Flipped && x < 0)
        {
            Flipped = true;
            root.localEulerAngles = Vector3.zero;
        }
        else if (Flipped && x > 0)
        {
            Flipped = false;
            root.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void Walk(bool walking)
    {
        anim.SetBool("walking", walking);
    }

    public void Attack(bool attacking, int attackType)
    {
        anim.SetInteger("AttackType", attackType);
        anim.SetBool("Attack", attacking);

        if (!attacking) anim.ResetTrigger("AttackTrigger");
        else anim.SetTrigger("AttackTrigger");
    }

    void EventFRAME_HitPicker()
    {
        SoundFX.Play_Action_Pinche();
    }
    void EventFRAME_Gloves()
    {
        //SoundFX.Play_Action_Take_With_Gloves();
    }
    void EventFRAME_HitShowel()
    {
        SoundFX.Play_Action_Showel_Hit();
    }
    void EventFRAME_Step()
    {
        SoundFX.Play_NextStep();
    }

    
}