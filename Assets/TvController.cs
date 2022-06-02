using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TvController : MonoBehaviour
{
   public AudioSource myAudioSource;
   public VideoPlayer myClip;

    private void Start()
    {
        myAudioSource.mute = true;
    }

    public void OpenTV()
    {
        myClip.Stop();
        myAudioSource.mute = false;
        UI_TeleController.instance.Open(CloseTV);
        myClip.Play();
    }
    public void CloseTV()
    {
        myAudioSource.mute = true;
        UI_TeleController.instance.Close();
    }
}
