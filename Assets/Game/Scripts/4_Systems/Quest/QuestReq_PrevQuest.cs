using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReq_PrevQuest : QuestRequirement
{
    [SerializeField] BaseQuest[] prevQuest = null;

    public override bool IsRequireComplete()
    {
        bool questComplete = true;
        for (int i = 0; i < prevQuest.Length; i++)
        {
            if(!prevQuest[i].IsQuestCompleted())
            {
                questComplete = false;
                break;
            }
        }
        return questComplete;
    }

    public override void RequireRemove(bool isOneShot)
    {
    }
}
