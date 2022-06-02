using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbacksMainMenu : MonoBehaviour
{
    public AnimEvent credits;
    public AnimEvent main;

    private void Start()
    {
        credits.ADD_ANIM_EVENT_LISTENER("character_enter", ANIM_EVENT_CharEnter);
        credits.ADD_ANIM_EVENT_LISTENER("character_exit", ANIM_EVENT_CharExit);
        main.ADD_ANIM_EVENT_LISTENER("menutransition_enter", ANIM_EVENT_Transition_enter);
        main.ADD_ANIM_EVENT_LISTENER("menutransition_exit", ANIM_EVENT_Transition_exit);
    }

    void ANIM_EVENT_CharEnter()
    {
        SoundFX.Play_Menu_WhoosCharacterEnter();
    }
    void ANIM_EVENT_CharExit()
    {
        SoundFX.Play_Menu_WhoosCharacterExit();
    }
    void ANIM_EVENT_Transition_enter()
    {
        SoundFX.Play_Transition_Enter();
    }
    void ANIM_EVENT_Transition_exit()
    {
        SoundFX.Play_Transition_Exit();
    }
}
