using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRew_Inventory : QuestReward
{
    [SerializeField] int slotsAmmount = 5;

    public override void ExecuteReward()
    {
        PlayerInventory.AddSlots(slotsAmmount);
    }
}
