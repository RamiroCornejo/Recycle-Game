using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseNode
{
    public Rect myRect;
    public Rect expandRect;
    public Rect minimizeRect;
    public string myName;
    public bool minimized;
    public NodeSave mySave;

    public BaseNode(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false)
    {
        myRect = new Rect(x, y, width, height);
        expandRect = new Rect(x, y, width, height);
        minimizeRect = new Rect(x, y, 150, 50);
        myName = nodeName;
        minimized = isMinimized;
        if (minimized)
        {
            myRect.width = minimizeRect.width;
            myRect.height = minimizeRect.height;
        }
    }

    public bool MouseOverCheck(Event ev, Vector2 graphPan)
    {
        if (myRect.Contains(ev.mousePosition - graphPan))
            return true;
        else
            return false;
    }

    public abstract BaseNode CloneNode();

    public abstract void DrawConections(Action<Vector3, Vector3> drawLine);

    public abstract void DeleteNode();

    public abstract void DeleteConnections(BaseNode node);

    public abstract Vector2 GetPos();
}

public class BaseDialogueNode : BaseNode
{
    public DialogParameters dialog = new DialogParameters();
    public BaseNode connect;
    public Vector2 posConnect;

    public BaseDialogueNode(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false) : base(x, y, width, height, nodeName, _parentNode, isMinimized)
    {
    }

    public BaseDialogueNode SetSave(BaseDialogueSaveData save)
    {
        dialog = save.dialog.Clone();
        return this;
    }

    public override void DeleteConnections(BaseNode node)
    {
        if (node == connect) connect = null;
    }

    public override void DeleteNode()
    {
        connect = null;
    }

    public override void DrawConections(Action<Vector3, Vector3> drawLine)
    {
        if (connect == null) return;
        Vector2 connectedNodeCenter = new Vector2(connect.myRect.position.x + connect.myRect.width / 2f,
                                          connect.myRect.position.y + connect.myRect.height / 2f);

        drawLine(posConnect, connectedNodeCenter);
    }

    public override Vector2 GetPos() => posConnect;

    public override BaseNode CloneNode()
    {
        var node = new BaseDialogueNode(myRect.x + 2, myRect.y + 2, myRect.width,myRect.height, myName, null, minimized);

        node.dialog = dialog.Clone();

        return node;
    }
}

public class AnswerDialogueNode : BaseNode
{
    public DialogParameters dialog = new DialogParameters();
    public List<Answer> answers = new List<Answer>();
    public float answerHeight = 50;
    public Answer selectAnswer = null;

    public AnswerDialogueNode(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false) : base(x, y, width, height, nodeName, _parentNode, isMinimized)
    {
    }

    public AnswerDialogueNode SetSave(AnswerDialogueSaveData save)
    {
        dialog = save.dialog.Clone();
        for (int i = 0; i < save.answerText.Count; i++)
        {
            answers.Add(new Answer());
            answers[i].answerText = save.answerText[i];
        }
        return this;
    }

    public override void DeleteNode()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].connection = null;
        }

        answers.Clear();
    }

    public override void DeleteConnections(BaseNode node)
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if(answers[i].connection == node) answers[i].connection = null;
        }
    }

    public override void DrawConections(Action<Vector3, Vector3> drawLine)
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if (answers[i].connection == null) continue;
            Vector2 connectedNodeCenter = new Vector2(answers[i].connection.myRect.position.x + answers[i].connection.myRect.width / 2f,
                                  answers[i].connection.myRect.position.y + answers[i].connection.myRect.height / 2f);

            drawLine(answers[i].posConnect, connectedNodeCenter);
        }
    }

    public override Vector2 GetPos()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if (answers[i] == selectAnswer) return answers[i].posConnect;
        }

        return Vector3.zero;
    }

    public override BaseNode CloneNode()
    {
        var node = new AnswerDialogueNode(myRect.x + 2, myRect.y + 2, myRect.width, myRect.height, myName, null, minimized);

        node.dialog = dialog.Clone();
        for (int i = 0; i < answers.Count; i++)
        {
            node.answers.Add(new Answer());
            node.answers[i].answerText = answers[i].answerText;
        }

        return node;
    }
}

public class ActionNodeWithConnection : BaseNode
{
    public Vector2 posConnect;
    public int ID;
    public BaseNode connect;

    public ActionNodeWithConnection(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false) : base(x, y, width, height, nodeName, _parentNode, isMinimized)
    {
    }

    public ActionNodeWithConnection SetSave(ActionNodeSaveData save)
    {
        ID = save.ID;
        myName = save.nodeName;

        return this;
    }

    public override void DeleteNode()
    {
        connect = null;
    }

    public override void DeleteConnections(BaseNode node)
    {
        if (node == connect) connect = null;
    }

    public override void DrawConections(Action<Vector3, Vector3> drawLine)
    {
        if (connect == null) return;
        Vector2 connectedNodeCenter = new Vector2(connect.myRect.position.x + connect.myRect.width / 2f,
                                          connect.myRect.position.y + connect.myRect.height / 2f);

        drawLine(posConnect, connectedNodeCenter);
    }

    public override BaseNode CloneNode()
    {
        var node = new ActionNodeWithConnection(myRect.x + 2, myRect.y + 2, myRect.width, myRect.height, myName, null, minimized);
        node.ID = ID;

        return node;
    }

    public override Vector2 GetPos() => posConnect;
}

public class RequirementNode : BaseNode
{
    public int ID;
    public Vector2 posConnectA;
    public Vector2 posConnectB;
    public BaseNode connectA;
    public BaseNode connectB;
    public int selectedOption = 0;

    public RequirementNode(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false) : base(x, y, width, height, nodeName, _parentNode, isMinimized)
    {
    }

    public RequirementNode SetSave(RequireNodeSaveData save)
    {
        ID = save.ID;
        selectedOption = save.selectedOption;
        myName = save.nodeName;
        return this;
    }

    public override void DeleteNode()
    {
        connectA = null;
        connectB = null;
    }

    public override void DeleteConnections(BaseNode node)
    {
        if (node == connectA) connectA = null;
        if (node == connectB) connectB = null;
    }

    public override void DrawConections(Action<Vector3, Vector3> drawLine)
    {
        if (connectA != null)
        {
            Vector2 connectedNodeCenter = new Vector2(connectA.myRect.position.x + connectA.myRect.width / 2f,
                                              connectA.myRect.position.y + connectA.myRect.height / 2f);

            drawLine(posConnectA, connectedNodeCenter);
        }

        if (connectB != null)
        {
            Vector2 connectedNodeCenter = new Vector2(connectB.myRect.position.x + connectB.myRect.width / 2f,
                                              connectB.myRect.position.y + connectB.myRect.height / 2f);

            drawLine(posConnectB, connectedNodeCenter);
        }
    }

    public override Vector2 GetPos()
    {
        if (selectedOption == 1) return posConnectA;
        else return posConnectB;
    }

    public override BaseNode CloneNode()
    {
        var node = new RequirementNode(myRect.x + 2, myRect.y + 2, myRect.width, myRect.height, myName, null, minimized);
        node.ID = ID;

        return node;
    }
}

public class DelayNode : BaseNode
{
    public float delayTime;
    public Vector2 posConnect;
    public BaseNode connect;

    public DelayNode(float x, float y, float width, float height, string nodeName, BaseNode _parentNode, bool isMinimized = false) : base(x, y, width, height, nodeName, _parentNode, isMinimized)
    {
    }

    public DelayNode SetSave(DelayNodeSaveData save)
    {
        delayTime = save.delayTime;
        return this;
    }

    public override void DeleteNode()
    {
        connect = null;
    }

    public override void DeleteConnections(BaseNode node)
    {
        if (node == connect) connect = null;
    }

    public override void DrawConections(Action<Vector3, Vector3> drawLine)
    {
        if (connect == null) return;
        Vector2 connectedNodeCenter = new Vector2(connect.myRect.position.x + connect.myRect.width / 2f,
                                          connect.myRect.position.y + connect.myRect.height / 2f);

        drawLine(posConnect, connectedNodeCenter);
    }

    public override Vector2 GetPos() => posConnect;

    public override BaseNode CloneNode()
    {
        var node = new DelayNode(myRect.x + 2, myRect.y + 2, myRect.width, myRect.height, myName, null, minimized);
        node.delayTime = delayTime;

        return node;
    }
}

public enum NodesType
{
    BaseDialogue,
    Answer,
    Action,
    Requirement,
    Delay
}