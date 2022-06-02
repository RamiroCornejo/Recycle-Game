using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRew_GetTool : QuestReward
{
    [SerializeField] CharacterTools toolToGet = CharacterTools.Gloves;

    public override void ExecuteReward()
    {
        Character.instance.AddTool(toolToGet);
    }
}
