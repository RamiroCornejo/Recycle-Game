using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent TriggerEnter = null;
    [SerializeField] UnityEvent TriggerExit = null;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Character>()) return;

        TriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Character>()) return;

        TriggerExit.Invoke();
    }
}
