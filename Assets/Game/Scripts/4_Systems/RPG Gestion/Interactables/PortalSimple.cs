using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSimple : MonoBehaviour
{
    public Transform target;

    public void Execute()
    {
        Character.TrackInput(false);
        Curtain.Open(OnFinishCurtain);
    }

    void OnFinishCurtain()
    {
        //Character.instance.controller.enabled = false;
        //Character.instance.controller.transform.position = target.transform.position;
        Invoke("OnFinishTeleport", 0.3f);
        Curtain.Close();
    }

    void OnFinishTeleport()
    {
        Character.TrackInput(true);
        //Character.instance.controller.enabled = true;
    }
}
