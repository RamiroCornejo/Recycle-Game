using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger_ForPLayer : MonoBehaviour
{
    public UnityEvent EV_TriggerEnter;
    public UnityEvent EV_TriggerExit;

    DoSomethingWithCharacter behaviour;

    private void Start()
    {
        behaviour = GetComponent<DoSomethingWithCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Character>();

        if (player != null)
        {
            EV_TriggerEnter.Invoke();
            if (behaviour) behaviour.CharacterEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Character>();

        if (player != null)
        {
            EV_TriggerExit.Invoke();
            if (behaviour) behaviour.CharacterExit();
        }
    }
}
