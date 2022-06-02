using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChestAnimation : MonoBehaviour
{
    public AnimEvent myAnimEvent;
    public Animator myAnimator;

    public bool IsAContainer;

    public void SubscribeToFinishOpen(Action callback_finish)
    {
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("endOpen", callback_finish);
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("open", EVENT_OpenAnimation);
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("close", EVENT_CloseAnimation);
    }

    public void Open()
    {
        myAnimator.SetBool("Open", true);

        if (!IsAContainer)
            SoundFX.Play_Storage_Open_Garbage();
        else
            SoundFX.Play_Storage_Open_Container();
    }
    public void Close()
    {
        myAnimator.SetBool("Open", false);
    }

    public void EVENT_OpenAnimation()
    {
        if (!IsAContainer)
            SoundFX.Play_Storage_Open_Garbage();
        else
            SoundFX.Play_Storage_Open_Container();
    }
    public void EVENT_CloseAnimation()
    {
        if (!IsAContainer)
            SoundFX.Play_Storage_Close_Garbage();
        else
            SoundFX.Play_Storage_Close_Container();
    }
}
