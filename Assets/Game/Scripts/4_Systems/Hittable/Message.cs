using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Message : MonoBehaviour
{
    [SerializeField] bool useTimer = false;
    [SerializeField] float timeToClose = 3;

    float timer;
    bool timerOn;

    public void SendText(string mess)
    {
        if (useTimer) { timer = 0; timerOn = true; }
        OnSendText(mess);
    }

    protected abstract void OnSendText(string mess);

    public void CloseText()
    {
        timer = 0;
        timerOn = false;
        OnCloseText();
    }

    protected abstract void OnCloseText();

    private void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            if (timer >= timeToClose)
                CloseText();
        }
    }
}
