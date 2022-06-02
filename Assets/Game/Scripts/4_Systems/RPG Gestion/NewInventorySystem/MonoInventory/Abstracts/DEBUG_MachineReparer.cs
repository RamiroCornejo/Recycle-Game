using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_MachineReparer : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.R))
        {
            var machines = FindObjectsOfType<QuestMachineHandler>();
            foreach (var m in machines)
            {
                m.ExecuteReward();
            }
        }
    }
}
