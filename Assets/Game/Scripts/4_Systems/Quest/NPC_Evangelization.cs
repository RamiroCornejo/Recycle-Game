using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Evangelization : QuestReward
{
    [SerializeField] MonoInvent_Base chestToDeposit = null;

    [SerializeField] TrashHittable hittable = null;

    [SerializeField] Spawner[] spawners = new Spawner[0];

    [SerializeField, Range(0, 1)] float percentToStack = 1;

    public override void ExecuteReward()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].DecreaseAppearProb(hittable, StackItemsInTheChest);
        }
    }

    int StackItemsInTheChest(int cant)
    {
        int ammountToStack = (int)(cant * percentToStack);

        var filter = chestToDeposit.GetComponent<FilterRecycled>();
        if (filter)
        {
            if (filter.CheckByElement(hittable.elementToRepresent))
            {
                Debug.Log("Entra");
                chestToDeposit.AddElement(hittable.elementToRepresent, ammountToStack, 1, true, false);
            }
        }
        else
        {
            chestToDeposit.AddElement(hittable.elementToRepresent, ammountToStack, 1, true, false);
        }

        

        return cant - ammountToStack;
    }
}
