using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class AnswerButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;
    int id;
    Action<int> SelectAns;


    public void SetAnswer(string txt, Action<int> action, int _id)
    {
        id = _id;
        SelectAns = action;

        text.text = txt;
    }

    public void Select()
    {
        SelectAns(id);
    }
}
