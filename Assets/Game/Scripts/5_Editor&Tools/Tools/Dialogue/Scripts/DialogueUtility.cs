using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueUtility : MonoBehaviour
{
    [SerializeField] SavedDialogue[] dialogues = new SavedDialogue[0];
    SavedDialogue currentDialogue = null;
    int currentIndex;
    [SerializeField] ActionDialogueBinder actionBinder = null;
    [SerializeField] FunctionDialogueBinder funcBinder = null;
    [SerializeField] GenericInteractable interact = null;
    [SerializeField] QuestReq_ObjCheck objCheck;
    NodeSave currentNode;

    [SerializeField] bool getInteractableAgain = true;

    [SerializeField] bool trackInput = true;

    private void Awake()
    {
        currentDialogue = dialogues[currentIndex];
    }
    public void StartDialogue(bool isInteractableAgain)
    {
        if(!DialogueUIDisplay.instance.OpenDialog(trackInput)) return;
        getInteractableAgain = isInteractableAgain;
        currentNode = currentDialogue.firstNode;

        ReadNode();
    }

    void NextDialogue(int answerIndex = -1)
    {
        if (GetNextNode(answerIndex))
        {
            ReadNode();
        }
        else
        {
            FinishDialog();
        }
    }

    public void SetInterctable(bool isInteractableAgain) => getInteractableAgain = isInteractableAgain;

    void FinishDialog()
    {
        DialogueUIDisplay.instance.CloseDialog(trackInput);
        currentNode = null;

        if (getInteractableAgain)
            GetInteractable();

        if (currentDialogue.nextDialogue)
            NextDialogueIndex();

        if (currentDialogue.repeatDialogue)
            interact.AddToInteractor(Character.instance.GetComponentInChildren<GenericInteractor>());
    }

    void GetInteractable()
    {
        var interactable = GetComponent<GenericInteractable>();

        interactable.AddToInteractor();
    }

    public void SkipDelay() => DialogueUIDisplay.instance.ForceEndDelay();

    public void NextDialogueIndex()
    {
        currentIndex += 1;

        if(currentIndex < dialogues.Length)
            currentDialogue = dialogues[currentIndex];
        else
            Debug.LogWarning("qué querés hacer pa");
    }

    public void SetDialogue(SavedDialogue dialogue)
    {
        currentDialogue = dialogue;
    }


    #region PARA LEER LOS NODOS

    void ReadNode()
    {
        switch (currentNode.myType)
        {
            case NodesType.BaseDialogue:
                ReadBaseDialog(currentNode.baseDialogueSD);
                break;

            case NodesType.Answer:
                ReadAnswerDialog(currentNode.answerDialogueSD);
                break;

            case NodesType.Action:
                ReadActionNode(currentNode.actionSD);
                break;

            case NodesType.Requirement:
                ReadRequireNode(currentNode.requireSD);
                break;

            case NodesType.Delay:
                ReadDelayNode(currentNode.delaySD);
                break;

            default:
                break;
        }
    }

    bool GetNextNode(int answerIndex = -1)
    {
        int ID = -1;
        switch (currentNode.myType)
        {
            case NodesType.BaseDialogue:
                ID = currentNode.baseDialogueSD.connect;

                break;
            case NodesType.Answer:
                ID = currentNode.answerDialogueSD.answerConnection[answerIndex];
                break;
            case NodesType.Action:
                ID = currentNode.actionSD.connect;
                break;
            case NodesType.Requirement:
                ID = answerIndex == 0 ? currentNode.requireSD.connectA : currentNode.requireSD.connectB;
                break;
            case NodesType.Delay:
                ID = currentNode.delaySD.connect;
                break;
            default:
                break;
        }
        if (ID == -1)
            return false;

        currentNode = currentDialogue.SearchNodeByID(ID);
        return true;
    }

    void ReadBaseDialog(BaseDialogueSaveData sd)
    {
        if (objCheck != null) sd.dialog.mainText = ReplaceWithItems(sd.dialog.mainText);
        DialogueUIDisplay.instance.ShowDialog(sd.dialog, NextDialogue);
    }

    string ReplaceWithItems(string original)
    {
        string textReplaced = original;
        string[] items = objCheck.GetItemsOnString();

        //Debug.Log("asd");
        for (int i = 0; i < items.Length; i++)
        {
            textReplaced = textReplaced.Replace("/item" + i, items[i]);
        }

        return textReplaced;
    }

    void ReadAnswerDialog(AnswerDialogueSaveData sd)
    {
        if (objCheck != null) sd.dialog.mainText = ReplaceWithItems(sd.dialog.mainText);
        DialogueUIDisplay.instance.ShowDialog(sd.dialog, NextDialogue);
        DialogueUIDisplay.instance.ShowAnswers(sd.answerText);
    }

    void ReadActionNode(ActionNodeSaveData sd)
    {
        actionBinder.ActionNode(sd.nodeName);

        NextDialogue();
    }

    void ReadRequireNode(RequireNodeSaveData sd)
    {
        bool condition = funcBinder.CheckCondition(sd.nodeName);

        if (condition) NextDialogue(0);
        else NextDialogue(1);
    }

    void ReadDelayNode(DelayNodeSaveData sd)
    {
        DialogueUIDisplay.instance.DisplayDelay(sd.delayTime, () => NextDialogue(-1));
    }

    #endregion

}
