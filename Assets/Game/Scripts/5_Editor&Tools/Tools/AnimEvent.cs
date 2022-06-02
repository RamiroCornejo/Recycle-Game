using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AnimEvent : MonoBehaviour
{
    Dictionary<string, Action> events = new Dictionary<string, Action>();

    public void ADD_ANIM_EVENT_LISTENER(string parameter, Action callback)
    {
        if (!events.ContainsKey(parameter))
        {
            events.Add(parameter, null);
            events[parameter] += callback;
        }
        else
        {
            events[parameter] += callback;
        }
    }
    public void REMOVE_ANIM_EVENT_LISTENER(string parameter, Action callback)
    {
        if (events.ContainsKey(parameter))
        {
            events[parameter] -= callback;
        }
    }

    public void TRIGGER_EVENT(string parameter)
    {
        //Debug.Log(name);

        if (events.ContainsKey(parameter))
            events[parameter]?.Invoke();
        else
            Debug.Log("eee");
    }
}
