using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseQuest : MonoBehaviour
{
    public bool questCompleted;
    [SerializeField] QuestRequirement requirement = null;
    [SerializeField] QuestReward reward = null;
    [SerializeField] bool isOneShot = true;

    [SerializeField] UnityEvent OnEndQuestDialogue = new UnityEvent();

    public bool IsQuestCompleted() => questCompleted;

    public bool IsQuestRequireComplete() => requirement.IsRequireComplete();

    public void RequestClear()
    {
        requirement.RequireRemove(isOneShot);
        if (isOneShot) questCompleted = true;
        DialogueUIDisplay.instance.SetEndDialogue(() => OnEndQuestDialogue.Invoke());
    }

    public void GetReward() => reward.ExecuteReward();

}
