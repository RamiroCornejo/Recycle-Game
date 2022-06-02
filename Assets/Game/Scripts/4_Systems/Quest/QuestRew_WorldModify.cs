using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRew_WorldModify : QuestReward
{
    [SerializeField] GameObject[] objectsToDesactive = new GameObject[0];
    [SerializeField] GameObject[] objectsToActive = new GameObject[0];

    public override void ExecuteReward()
    {
        for (int i = 0; i < objectsToDesactive.Length; i++)
        {
            objectsToDesactive[i].SetActive(false);
        }

        for (int i = 0; i < objectsToActive.Length; i++)
        {
            objectsToActive[i].SetActive(true);
        }
    }
}
