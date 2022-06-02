using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_TeleController : MonoBehaviour
{
    public static UI_TeleController instance;
    private void Awake() => instance = this;

    [SerializeField] CanvasGroup myCanvasGroup;
    Action callback_close;

    public void Open(Action callback_close)
    {
        myCanvasGroup.alpha = 1;
        myCanvasGroup.blocksRaycasts = true;
        myCanvasGroup.interactable = true;
        this.callback_close = callback_close;
    }
    public void Close()
    {
        myCanvasGroup.alpha = 0;
        myCanvasGroup.blocksRaycasts = false;
        myCanvasGroup.interactable = false;
    }

    public void BUTTON_Close()
    {
        callback_close?.Invoke();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
        {
            BUTTON_Close();
        }
    }

}
