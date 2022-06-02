using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public static MenuButtons instance;
    [SerializeField] Credits credits = null;
    [SerializeField] int sceneIndex = 1;

    [SerializeField] Animator firstScreenAnim = null;
    [SerializeField] Animator tentacleAnim = null;
    [SerializeField] Animator mainHudAnim = null;
    [SerializeField] Animator creditsAnimator = null;
    [SerializeField] GameObject contratCredits = null;
    [SerializeField] AnimEvent[] events = null;
    [SerializeField] TitoideMenu tito = null;
    [SerializeField] AnimEvent splashEvents;
    private void Awake()
    {
        instance = this;
    }

    bool firstScreenActive;
    public bool onCredits = false;

    bool onClick;

    private void Start()
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].ADD_ANIM_EVENT_LISTENER("AppearEvent", AppearOver);
            events[i].ADD_ANIM_EVENT_LISTENER("DissappearEvent", DissappearOver);
        }
        splashEvents.ADD_ANIM_EVENT_LISTENER("splash_go", ANIM_EVENT_SpashGo);
    }

    void ANIM_EVENT_SpashGo()
    {
        SoundFX.Play_Splash_GO();
    }

    public void StartCredits()
    {
        if (onCredits)
        {
            creditsAnimator.Play("Dissappear");
        }
        else
        {
            mainHudAnim.Play("Dissappear");
        }
    }

    public void Play()
    {
        if (onClick) return;

        onClick = true;
        tentacleAnim.Play("SelloPlay");
    }

    public void Credits()
    {
        if (onClick) return;

        onClick = true;
        tentacleAnim.Play("SelloCredits");
    }

    private void Update()
    {
        if(!firstScreenActive && Input.anyKey)
        {
            QuitPressButtonScreen();
        }
    }

    void QuitPressButtonScreen()
    {
        firstScreenActive = true;
        firstScreenAnim.Play("Quit");
        //mainHudAnim.Play("Appear");
        tito.gameObject.SetActive(true);
    }

    void DissappearOver()
    {
        if (onCredits)
        {
            mainHudAnim.Play("Appear");
        }
        else
        {
            creditsAnimator.Play("Appear");
        }
    }

    void AppearOver()
    {
        contratCredits.SetActive(false);
        onCredits = !onCredits;
        onClick = false;
    }
}
