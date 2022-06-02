using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMachineHandler : QuestReward
{
    [SerializeField] bool startRepared = false;

    [SerializeField] Collider machineInteract = null;
    [SerializeField] GameObject reparedModel;
    [SerializeField] Collider questInteract = null;
    [SerializeField] GameObject breakModel = null;
    [SerializeField] DialogueUtility myDialog = null;

    private void Start()
    {
        if (startRepared)
            ExecuteReward();
        else
        {
            Invoke("Broke", 0.2f);
        }
    }

    void Broke()
    {
        questInteract.enabled = true;
        breakModel.SetActive(true);
        myDialog.SetInterctable(true);
        machineInteract.enabled = false;
        reparedModel.SetActive(false);
    }

    

    public override void ExecuteReward()
    {
        questInteract.enabled = false;
        breakModel.SetActive(false);
        myDialog.SetInterctable(false);
        machineInteract.enabled = true;
        reparedModel.SetActive(true);
    }
}
