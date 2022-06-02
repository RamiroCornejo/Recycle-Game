using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRew_GetObj : QuestReward
{
    [SerializeField] ObjectData[] objectsToGive = new ObjectData[0];
     

    public override void ExecuteReward()
    {
            Vector3 spawnPos = transform.position - transform.forward + Vector3.up;
        for (int i = 0; i < objectsToGive.Length; i++)
        {

            for (int x = 0; x < objectsToGive[i].ammount; x++)
            {
                var newItem = GameManager.instance.GetItem(spawnPos);
                newItem.SetData(objectsToGive[i].element, false, objectsToGive[i].isDry, objectsToGive[i].isDirty);
                spawnPos += Vector3.up;
            }
        }
    }
}
