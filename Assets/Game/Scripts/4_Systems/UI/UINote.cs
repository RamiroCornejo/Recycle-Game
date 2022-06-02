using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINote : MonoBehaviour
{
    [SerializeField] DialogueUtility dialog = null;
    [SerializeField] float scaleMagnitude = 2;

    public void FeedbackOnEnter()
    {
        transform.localScale = Vector3.one * scaleMagnitude;
    }

    public void FeedbackOnExit()
    {
        transform.localScale = Vector3.one;
    }

    public void StartDialog()
    {
        dialog.StartDialogue(false);
    }
}
