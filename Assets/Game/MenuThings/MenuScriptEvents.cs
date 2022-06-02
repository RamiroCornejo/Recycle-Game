using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScriptEvents : MonoBehaviour
{
    [SerializeField] GameObject contrat = null;
    [SerializeField] MenuButtons mainMenu = null;
    [SerializeField] GameObject creditsContrat = null;

    AnimEvent animevent;

    private void Start()
    {
        animevent = GetComponent<AnimEvent>();
        animevent.ADD_ANIM_EVENT_LISTENER("StampBegin", ANIM_EVENT_StampBegin);
        animevent.ADD_ANIM_EVENT_LISTENER("SelloIdle_go", ANIM_EVENT_StampIdle_Go);
        animevent.ADD_ANIM_EVENT_LISTENER("SelloIdle_back", ANIM_EVENT_StampIdle_Back);
    }

    void ANIM_EVENT_StampBegin()
    {
        SoundFX.Play_Menu_Sello();
    }
    void ANIM_EVENT_StampIdle_Go()
    {
        if(MenuButtons.instance.onCredits) SoundFX.Play_Tentacle_Idle_Go();

    }
    void ANIM_EVENT_StampIdle_Back()
    {
        if (MenuButtons.instance.onCredits) SoundFX.Play_Tentacle_Idle_Back();
    }

    public void PlayContrat()
    {
        contrat.SetActive(true);
       
    }

    public void PlayAnimOver()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsContrat()
    {
        creditsContrat.SetActive(true);
    }

    public void StartCreditsEvent()
    {
        mainMenu.StartCredits();
    }
}
