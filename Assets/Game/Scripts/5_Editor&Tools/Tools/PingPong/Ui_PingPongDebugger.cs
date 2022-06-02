using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ui_PingPongDebugger : MonoBehaviour
{
    PingPongLerp anim_ping = new PingPongLerp();
    PingPongLerp anim_pong = new PingPongLerp();
    public Image ping;
    public Image pong;
    public Image AnimValue;

    [SerializeField] TextMeshProUGUI nametext;
    public void Configure(string namevalue)
    {
        nametext.text = namevalue;

        anim_ping
            .Configure_TEMPLATE_ONESHOT_EXPLOSION(0.1f, 0.3f)
            .Configure_Callback(LerpPing);

        anim_pong
            .Configure_TEMPLATE_ONESHOT_EXPLOSION(0.1f, 0.3f)
            .Configure_Callback(LerpPong);
    }

    public void LerpValue(float val)
    {
        AnimValue.fillAmount = val;
    }
    public void ExecutePing()
    {
        anim_ping.Play();
    }
    public void ExecutePong()
    {
        anim_pong.Play();
    }

    void LerpPing(float val)
    {
        ping.color = Color.Lerp(Color.green, Color.black,val);
    }
    void LerpPong(float val)
    {
        pong.color = Color.Lerp(Color.green, Color.black, val);
    }

    

    void Update()
    {
        anim_ping.Tick(Time.deltaTime);
        anim_pong.Tick(Time.deltaTime);
    }
}
