using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDialogueBinder : MonoBehaviour
{
    [SerializeField] String_UnityEvDictionary actionDictionary = new String_UnityEvDictionary();

    public void ActionNode(string _name)
    {
        if (actionDictionary.ContainsKey(_name))
        {
            actionDictionary[_name].Invoke();
        }
    }
}
