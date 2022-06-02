using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMessage : Message
{
    protected override void OnCloseText()
    {
    }

    protected override void OnSendText(string mess)
    {
        UIManager.instance.SendText(mess);
    }
}
