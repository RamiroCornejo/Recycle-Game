using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeSave
{
    public Rect myRect;
    public Rect expandRect;
    public string myName;
    public bool minimized;
    [HideInInspector] public BaseDialogueSaveData baseDialogueSD = null;
    [HideInInspector] public AnswerDialogueSaveData answerDialogueSD = null;
    [HideInInspector] public ActionNodeSaveData actionSD = null;
    [HideInInspector] public RequireNodeSaveData requireSD = null;
    [HideInInspector] public DelayNodeSaveData delaySD = null;
    [HideInInspector] public NodesType myType;

    public BaseNode myNode;
    public int ID;

    public NodeSave(BaseNode node)
    {
        myNode = node;
        myRect = node.myRect;
        expandRect = node.expandRect;
        myName = node.myName;
        minimized = node.minimized;

        Type nodeType = node.GetType();

        if (nodeType == typeof(BaseDialogueNode))
        {
            baseDialogueSD = new BaseDialogueSaveData();
            baseDialogueSD.Save(node as BaseDialogueNode);
            myType = NodesType.BaseDialogue;
        }
        else if (nodeType == typeof(AnswerDialogueNode))
        {
            answerDialogueSD = new AnswerDialogueSaveData();
            answerDialogueSD.Save(node as AnswerDialogueNode);
            myType = NodesType.Answer;
        }
        else if (nodeType == typeof(ActionNodeWithConnection))
        {
            actionSD = new ActionNodeSaveData();
            actionSD.Save(node as ActionNodeWithConnection);
            myType = NodesType.Action;
        }
        else if (nodeType == typeof(RequirementNode))
        {
            requireSD = new RequireNodeSaveData();
            requireSD.Save(node as RequirementNode);
            myType = NodesType.Requirement;
        }
        else if (nodeType == typeof(DelayNode))
        {
            delaySD = new DelayNodeSaveData();
            delaySD.Save(node as DelayNode);
            myType = NodesType.Delay;
        }
    }

    public void SaveConnections()
    {
        Type nodeType = myNode.GetType();

        if (nodeType == typeof(BaseDialogueNode))
        {
            var tempNode = myNode as BaseDialogueNode;
            if (tempNode.connect == null) baseDialogueSD.connect = -1;
            else baseDialogueSD.connect = tempNode.connect.mySave.ID;
        }
        else if (nodeType == typeof(AnswerDialogueNode))
        {
            var tempNode = myNode as AnswerDialogueNode;
            for (int i = 0; i < tempNode.answers.Count; i++)
            {
                if (tempNode.answers[i].connection == null) answerDialogueSD.answerConnection.Add(-1);
                else answerDialogueSD.answerConnection.Add(tempNode.answers[i].connection.mySave.ID);
            }
        }
        else if (nodeType == typeof(ActionNodeWithConnection))
        {
            var tempNode = myNode as ActionNodeWithConnection;
            if (tempNode.connect == null) actionSD.connect = -1;
            else actionSD.connect = tempNode.connect.mySave.ID;
        }
        else if (nodeType == typeof(RequirementNode))
        {
            var tempNode = myNode as RequirementNode;
            if (tempNode.connectA == null) requireSD.connectA = -1;
            else requireSD.connectA = tempNode.connectA.mySave.ID;
            if (tempNode.connectB == null) requireSD.connectB = -1;
            else requireSD.connectB = tempNode.connectB.mySave.ID;
        }
        else if (nodeType == typeof(DelayNode))
        {
            var tempNode = myNode as DelayNode;
            if (tempNode.connect == null) delaySD.connect = -1;
            else delaySD.connect = tempNode.connect.mySave.ID;
        }
    }

    public BaseNode LoadNode()
    {
        if (myType == NodesType.BaseDialogue)
        {
            myNode = new BaseDialogueNode(myRect.x, myRect.y, expandRect.width, expandRect.height, myName, null, minimized).SetSave(baseDialogueSD);
        }
        else if (myType == NodesType.Answer)
        {
            myNode = new AnswerDialogueNode(myRect.x, myRect.y, expandRect.width, expandRect.height, myName, null, minimized).SetSave(answerDialogueSD);
        }
        else if (myType == NodesType.Action)
        {
            myNode = new ActionNodeWithConnection(myRect.x, myRect.y, expandRect.width, expandRect.height, myName, null, minimized).SetSave(actionSD);
        }
        else if (myType == NodesType.Requirement)
        {
            myNode = new RequirementNode(myRect.x, myRect.y, expandRect.width, expandRect.height, myName, null, minimized).SetSave(requireSD);
        }
        else if (myType == NodesType.Delay)
        {
            myNode = new DelayNode(myRect.x, myRect.y, expandRect.width, expandRect.height, myName, null, minimized).SetSave(delaySD);
        }

        return myNode;
    }

    public void LoadConnections(SavedDialogue save)
    {
        Type nodeType = myNode.GetType();

        if (nodeType == typeof(BaseDialogueNode))
        {
            var tempNode = myNode as BaseDialogueNode;
            if (baseDialogueSD.connect != -1) tempNode.connect = save.allNodes[baseDialogueSD.connect].myNode;
        }
        else if (nodeType == typeof(AnswerDialogueNode))
        {
            var tempNode = myNode as AnswerDialogueNode;
            for (int i = 0; i < answerDialogueSD.answerConnection.Count; i++)
            {
                if (answerDialogueSD.answerConnection[i] != -1) tempNode.answers[i].connection = save.allNodes[answerDialogueSD.answerConnection[i]].myNode;
            }
        }
        else if (nodeType == typeof(ActionNodeWithConnection))
        {
            var tempNode = myNode as ActionNodeWithConnection;
            if (actionSD.connect != -1) tempNode.connect = save.allNodes[actionSD.connect].myNode;
        }
        else if (nodeType == typeof(RequirementNode))
        {
            var tempNode = myNode as RequirementNode;
            if (requireSD.connectA != -1) tempNode.connectA = save.allNodes[requireSD.connectA].myNode;
            if (requireSD.connectB != -1) tempNode.connectB = save.allNodes[requireSD.connectB].myNode;
        }
        else if (nodeType == typeof(DelayNode))
        {
            var tempNode = myNode as DelayNode;
            if (delaySD.connect != -1) tempNode.connect = save.allNodes[delaySD.connect].myNode;
        }
    }
}

[Serializable]
public class DialogParameters
{
    public string mainText = "";
    public string titleText = "";
    public Sprite mySprite = null;
    public int spriteDir = 0;

    public DialogParameters Clone()
    {
        var newParams = new DialogParameters();
        newParams.mainText = mainText;
        newParams.titleText = titleText;
        newParams.spriteDir = spriteDir;
        newParams.mySprite = mySprite;
        return newParams;
    }
}

[Serializable]
public class BaseDialogueSaveData
{
    public DialogParameters dialog = new DialogParameters();
    public int connect;

    public void Save(BaseDialogueNode node)
    {
        dialog.mainText = node.dialog.mainText;
        dialog.titleText = node.dialog.titleText;
        dialog.spriteDir = node.dialog.spriteDir;
        dialog.mySprite = node.dialog.mySprite;
    }
}

[Serializable]
public class AnswerDialogueSaveData
{
    public DialogParameters dialog = new DialogParameters();
    public List<string> answerText = new List<string>();
    public List<int> answerConnection = new List<int>();

    public void Save(AnswerDialogueNode node)
    {
        dialog.mainText = node.dialog.mainText;
        dialog.titleText = node.dialog.titleText;
        dialog.spriteDir = node.dialog.spriteDir;
        dialog.mySprite = node.dialog.mySprite;
        for (int i = 0; i < node.answers.Count; i++)
            answerText.Add(node.answers[i].answerText);
    }
}

[Serializable]
public class ActionNodeSaveData
{
    public int ID;
    public int connect;
    public string nodeName;

    public void Save(ActionNodeWithConnection node)
    {
        ID = node.ID;
        nodeName = node.myName;
    }
}

[Serializable]
public class RequireNodeSaveData
{
    public int ID;
    public int connectA;
    public int connectB;
    public int selectedOption = 0;
    public string nodeName;

    public void Save(RequirementNode node)
    {
        ID = node.ID;
        selectedOption = node.selectedOption;
        nodeName = node.myName;
    }
}

[Serializable]
public class DelayNodeSaveData
{
    public float delayTime;
    public int connect;

    public void Save(DelayNode node)
    {
        delayTime = node.delayTime;
    }
}
