using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class DialgoueEditor : EditorWindow
{
    float toolbarHeight = 100;
    List<BaseNode> myNodes = new List<BaseNode>();

    SavedDialogue currentDialogue;

    BaseNode selectNode;
    BaseNode firstNode;

    bool panningScreen;
    Rect graphRect;
    Vector2 graphPan;
    Vector2 prevGraphPan;
    Vector2 mousePositionDown;
    List<BaseNode> selectedNodes = new List<BaseNode>();

    Action<BaseNode> SelectNodeAction;

    [MenuItem("Tools/DialogTool")]
    public static void OpenWindow()
    {
        var window = GetWindow(typeof(DialgoueEditor)) as DialgoueEditor;
        window.Show();
    }

    private void OnEnable()
    {
        graphRect = new Rect(0, 0, 100000, 100000);
        graphPan = new Vector2(0, 0);
    }
    private void Update()
    {
        Repaint();
    }
    private void OnGUI()
    {
        MouseInputsCheck(Event.current);
        currMousePos = Event.current.mousePosition;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);

        graphRect.x = graphPan.x;
        graphRect.y = graphPan.y;

        EditorGUILayout.BeginVertical(GUILayout.Height(position.height - toolbarHeight));

        GUI.BeginGroup(graphRect);
        BeginWindows();

        for (int i = 0; i < myNodes.Count; i++)
        {
            myNodes[i].DrawConections(Handles.DrawLine);
        }

        if(selectNode != null)
        {
            Handles.DrawLine(selectNode.GetPos(), currMousePos - graphPan);
        }

        var originalColor = GUI.backgroundColor;
        for (int i = 0; i < myNodes.Count; i++)
        {
            if (firstNode == myNodes[i])
                GUI.backgroundColor = new Color(0, 0.8f, 0.8f, 1);

            if(selectedNodes.Contains(myNodes[i]))
                EditorGUI.DrawRect(new Rect(myNodes[i].myRect.x - 2, myNodes[i].myRect.y - 2, myNodes[i].myRect.width + 4, myNodes[i].myRect.height + 4), Color.green);
            
            myNodes[i].myRect = GUI.Window(i, myNodes[i].myRect, DrawNode, myNodes[i].myName);

            GUI.backgroundColor = originalColor;
        }

        EndWindows();
        GUI.EndGroup();

        EditorGUILayout.EndVertical();

        if (selecting)
        {
            selectRect.size = new Vector2(Event.current.mousePosition.x - selectRect.x ,Event.current.mousePosition.y - selectRect.y);
            EditorGUI.DrawRect(selectRect, new Color(0, 0.7f, 0, 0.4f));
        }

        EditorGUILayout.BeginVertical(GUILayout.Height(toolbarHeight)); 

        EditorGUI.DrawRect(new Rect(0, 0, position.width, toolbarHeight), new Color(0.8f, 0.8f, 0.8f, 1));

        EditorGUILayout.LabelField("Ventana de nodos:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        currentDialogue = (SavedDialogue)EditorGUILayout.ObjectField("Current Dialogue Tree", currentDialogue, typeof(SavedDialogue), false);

        if (GUI.Button(GUILayoutUtility.GetRect(50, 20), "Load"))
            LoadDialogue(currentDialogue);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        if(myNodes.Count > 0)
        {
            if (firstNode == null) firstNode = myNodes[0];
            if (GUI.Button(GUILayoutUtility.GetRect(60, 30), "Save Dialogue Tree"))
                SaveDialogue(currentDialogue);

            if (GUI.Button(GUILayoutUtility.GetRect(60, 30), "Save Dialogue Tree as"))
                SaveDialogue(null);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

    }

    Rect selectRect;
    bool selecting;

    void MouseInputsCheck(Event current)
    {
        if (focusedWindow != this || mouseOverWindow != this) return; //Si el usuario no está en foco de la ventana, no chequeo los inputs

        if (current.button == 2 && current.type == EventType.MouseDown) //Si apretamos la ruedita del mouse...
        {
            panningScreen = true; //Empezamos a panear
            mousePositionDown = current.mousePosition; //Guardamos la posición de mi mouse para sacar cuentas
            prevGraphPan = new Vector2(graphPan.x, graphPan.y); //Y guardamos en qué posición estaba mi grafo
        }
        else if (current.button == 2 && current.type == EventType.MouseUp)
            panningScreen = false;

        if (panningScreen)
        {
            float x = prevGraphPan.x + current.mousePosition.x - mousePositionDown.x;
            graphPan.x = x > 0 ? 0 : x;

            float y = prevGraphPan.y + current.mousePosition.y - mousePositionDown.y;
            graphPan.y = y > 0 ? 0 : y;

            Repaint();
        }

        BaseNode overNode = null;
        for (int i = 0; i < myNodes.Count; i++)
        {
            if (myNodes[i].MouseOverCheck(current, graphPan))
                overNode = myNodes[i];
        }

        if (current.button == 1 && current.type == EventType.MouseDown)
        {
            currMousePos = current.mousePosition - graphPan;
            if (overNode != null)
                ContextMenuOverNode(new List<BaseNode>() { overNode });
            else
                ContextMenuNormal(null);
        }

        if (current.button == 0 && current.type == EventType.MouseDown)
        {
            if(selectNode != null && selectNode != overNode)
            {
                SelectNodeAction?.Invoke(overNode);
                selectNode = null;
            }

            if (!selectedNodes.Contains(overNode))
                selectedNodes = new List<BaseNode>();
        }

        if (current.button == 0 && current.type == EventType.MouseDown && overNode == null)
        {
            selecting = true;
            selectRect = new Rect(current.mousePosition, Vector2.zero);
        }
        else if (current.button == 0 && current.type == EventType.MouseUp && selecting)
        {
            selecting = false;

            for (int i = 0; i < myNodes.Count; i++)
            {
                if (selectRect.Contains(new Vector2(myNodes[i].myRect.x + myNodes[i].myRect.width / 2, myNodes[i].myRect.y + myNodes[i].myRect.height / 2) + graphPan, true))
                    selectedNodes.Add(myNodes[i]);
            }
        }
    }
    Vector2 currMousePos;

    void ContextMenuOverNode(List<BaseNode> overNode)
    {
        GenericMenu menu = new GenericMenu();
        if(overNode.Count == 1)menu.AddItem(new GUIContent("Set as First Node"), false, () => firstNode = overNode[0]);
        menu.AddItem(new GUIContent("Delete Node"), false, ()=> DeleteNode(overNode));

        menu.AddItem(new GUIContent("Clone Node"), false, () => CloneNodes(overNode));

        menu.ShowAsContext();
    }

    void CloneNodes(List<BaseNode> overNode)
    {
        for (int i = 0; i < overNode.Count; i++)
        {
            var newNode = overNode[i].CloneNode();
            myNodes.Add(newNode);

            Repaint();
        }
    }

    void ContextMenuNormal(BaseNode parent)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create Node/Dialogue Node"), false, ()=>AddDialogueNode(parent));
        menu.AddItem(new GUIContent("Create Node/Dialogue Node with Options"), false, ()=> AddAnswerNode(parent));
        menu.AddItem(new GUIContent("Create Node/Action Node"), false, () => AddActionNode(parent));
        menu.AddItem(new GUIContent("Create Node/Requirement Node"), false, () => AddRequirementNode(parent));
        menu.AddItem(new GUIContent("Create Node/Delay Node"), false, () => AddDelayNode(parent));

        menu.ShowAsContext();
    }

    #region Create Nodes

    void AddDialogueNode(BaseNode parent)
    {
        var newNode = new BaseDialogueNode(currMousePos.x, currMousePos.y, 300, 300, "Base Dialogue", parent);
        myNodes.Add(newNode);

        Repaint();
    }

    void AddAnswerNode(BaseNode parent)
    {
        var newNode = new AnswerDialogueNode(currMousePos.x, currMousePos.y, 300, 280, "Answer", parent);
        myNodes.Add(newNode);

        Repaint();
    }

    void AddActionNode(BaseNode parent)
    {
        var newNode = new ActionNodeWithConnection(currMousePos.x, currMousePos.y, 250, 120, "Action", parent);
        myNodes.Add(newNode);

        Repaint();
    }

    void AddRequirementNode(BaseNode parent)
    {
        var newNode = new RequirementNode(currMousePos.x, currMousePos.y, 250, 150, "Requirement", parent);
        myNodes.Add(newNode);

        Repaint();
    }

    void AddDelayNode(BaseNode parent)
    {
        var newNode = new DelayNode(currMousePos.x, currMousePos.y, 250, 100, "Delay", parent);
        myNodes.Add(newNode);

        Repaint();
    }

    #endregion

    void DeleteNode(List<BaseNode> nodeToDelete)
    {
        for (int i = 0; i < nodeToDelete.Count; i++)
        {
            nodeToDelete[i].DeleteNode();
            for (int x = 0; x < myNodes.Count; x++)
            {
                myNodes[x].DeleteConnections(nodeToDelete[i]);
            }
            if (firstNode == nodeToDelete[i]) firstNode = null;
            myNodes.Remove(nodeToDelete[i]);
            Repaint();
        }
        selectedNodes.Clear();
    }

    void SaveDialogue(SavedDialogue saveData)
    {
        if(saveData == null)
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Dialogue Tree", "NewSavedDialogues", "asset", "");

            if (path == null || path == "")
                return;

            currentDialogue = ScriptableUltility.CreateScriptable<SavedDialogue>(path);
        }

        currentDialogue.allNodes = new NodeSave[myNodes.Count];

        for (int i = 0; i < myNodes.Count; i++)
        {
            NodeSave newNode = new NodeSave(myNodes[i]);
            newNode.ID = i;
            myNodes[i].mySave = newNode;
            currentDialogue.allNodes[i] = newNode;

            if (myNodes[i] == firstNode) currentDialogue.firstNode = newNode;
        }

        for (int i = 0; i < myNodes.Count; i++)
        {
            currentDialogue.allNodes[i].SaveConnections();
        }

        EditorUtility.SetDirty(currentDialogue);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Repaint();
    }

    void LoadDialogue(SavedDialogue saveData)
    {
        if(saveData == null)
        {
            EditorUtility.DisplayDialog("Error", "There is no dialogue to load.", "Ok");
        }
        else
        {
            myNodes.Clear();
            firstNode = null;

            for (int i = 0; i < saveData.allNodes.Length; i++)
            {
                var newNode = saveData.allNodes[i].LoadNode();
                newNode.mySave = saveData.allNodes[i];
                if (saveData.allNodes[i] == saveData.firstNode) firstNode = newNode;

                myNodes.Add(newNode);
            }

            for (int i = 0; i < saveData.allNodes.Length; i++)
            {
                saveData.allNodes[i].LoadConnections(saveData);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Repaint();
        }
    }

    #region Draw Nodes
    void DrawNode(int id)
    {
        Type nodeType = myNodes[id].GetType();

        if (nodeType == typeof(BaseDialogueNode))
            DrawDialogueNode(myNodes[id] as BaseDialogueNode);
        else if (nodeType == typeof(AnswerDialogueNode))
            DrawDialogueNodeAnswers(myNodes[id] as AnswerDialogueNode);
        else if (nodeType == typeof(ActionNodeWithConnection))
            DrawActionWConNode(myNodes[id] as ActionNodeWithConnection);
        else if (nodeType == typeof(RequirementNode))
            DrawRequirementNode(myNodes[id] as RequirementNode);
        else if (nodeType == typeof(DelayNode))
            DrawDelayNode(myNodes[id] as DelayNode);
    }

    void DrawDialogueNode(BaseDialogueNode node)
    {
        if (node.minimized)
        {
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = false;
                node.myRect.height = node.expandRect.height;
                node.myRect.width = node.expandRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Expand");
            node.posConnect = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                                          node.myRect.position.y + node.myRect.height / 2f);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = true;
                node.myRect.height = node.minimizeRect.height;
                node.myRect.width = node.minimizeRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Minimize");

            EditorGUILayout.EndHorizontal();

            node.dialog.titleText = EditorGUILayout.TextField("Title:", node.dialog.titleText);

            node.dialog.mySprite = EditorGUILayout.ObjectField("Sprite:", node.dialog.mySprite, typeof(Sprite), false) as Sprite;

            node.dialog.spriteDir = EditorGUILayout.IntSlider("Sprite Pos:", node.dialog.spriteDir, 0, 1);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Main Dialogue:", GUILayout.Width(145));
            node.dialog.mainText = EditorGUILayout.TextArea(node.dialog.mainText, GUILayout.Height(100), GUILayout.Width(140));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Connection:", GUILayout.Width(145));
            Color temp = GUI.color;
            if (selectNode == node) GUI.color = Color.yellow;
            else if (node.connect != null) GUI.color = Color.green;
            Rect r = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

            if (GUI.Button(r, ""))
            {
                selectNode = node;
                SelectNodeAction = (x) => node.connect = x;
            }
            GUI.color = temp;


            if (Event.current.type == EventType.Repaint) node.posConnect = node.myRect.position + r.position + new Vector2(10, 10);
            EditorGUILayout.EndHorizontal();
        }


        if (panningScreen) return;

        GUI.DragWindow();

        if (node.myRect.x < 0)
            node.myRect.x = 0;

        if (node.myRect.y < toolbarHeight)
            node.myRect.y = toolbarHeight;
    }

    void DrawDialogueNodeAnswers(AnswerDialogueNode node)
    {
        if (node.minimized)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = false;
                node.myRect.height = node.expandRect.height;
                node.myRect.width = node.expandRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Expand");

            for (int i = 0; i < node.answers.Count; i++)
            {
                node.answers[i].posConnect = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                                          node.myRect.position.y + node.myRect.height / 2f);
            }

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = true;
                node.expandRect = node.myRect;
                node.myRect.height = node.minimizeRect.height;
                node.myRect.width = node.minimizeRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Minimize");

            EditorGUILayout.EndHorizontal();
            node.dialog.titleText = EditorGUILayout.TextField("Title:", node.dialog.titleText);

            node.dialog.mySprite = EditorGUILayout.ObjectField("Sprite:", node.dialog.mySprite, typeof(Sprite), false) as Sprite;

            node.dialog.spriteDir = EditorGUILayout.IntSlider("Sprite Pos:", node.dialog.spriteDir, 0, 1);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Main Dialogue:", GUILayout.Width(145));
            node.dialog.mainText = EditorGUILayout.TextArea(node.dialog.mainText, GUILayout.Height(100), GUILayout.Width(140));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            for (int i = 0; i < node.answers.Count; i++)
            {
                node.answers[i].answerText = EditorGUILayout.TextField("Answer Text:", node.answers[i].answerText);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Delete Answer", GUILayout.Width(130)))
                {
                    if (node.selectAnswer == node.answers[i]) node.selectAnswer = null;
                    node.answers.RemoveAt(i);
                    node.myRect.height -= node.answerHeight;
                    node.expandRect.height -= node.answerHeight;
                    Repaint();
                    return;
                }
                Color temp = GUI.color;
                if (selectNode == node && node.selectAnswer == node.answers[i]) GUI.color = Color.yellow;
                else if (node.answers[i].connection != null) GUI.color = Color.green;
                Rect r = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

                if (GUI.Button(r, ""))
                {
                    node.selectAnswer = node.answers[i];
                    selectNode = node;
                    SelectNodeAction = (x) => node.selectAnswer.connection = x;
                }
                GUI.color = temp;


                if (Event.current.type == EventType.Repaint) node.answers[i].posConnect = node.myRect.position + r.position + new Vector2(10, 10);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            if (GUILayout.Button("Add Answer"))
            {
                node.answers.Add(new Answer());
                node.myRect.height += node.answerHeight;
                node.expandRect.height += node.answerHeight;
                Repaint();
                return;
            }
        }
        if (panningScreen) return;

        GUI.DragWindow();

        if (node.myRect.x < 0)
            node.myRect.x = 0;

        if (node.myRect.y < toolbarHeight)
            node.myRect.y = toolbarHeight;
    }

    void DrawActionWConNode(ActionNodeWithConnection node)
    {
        if (node.minimized)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = false;
                node.myRect.height = node.expandRect.height;
                node.myRect.width = node.expandRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Expand");

            node.posConnect = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                              node.myRect.position.y + node.myRect.height / 2f);

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = true;
                node.myRect.height = node.minimizeRect.height;
                node.myRect.width = node.minimizeRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Minimize");

            EditorGUILayout.EndHorizontal();
            node.myName = EditorGUILayout.TextField("Node Name:", node.myName);
            node.ID = EditorGUILayout.IntField("ID:", node.ID);

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Connection:", GUILayout.Width(145));
            Color temp = GUI.color;
            if (selectNode == node) GUI.color = Color.yellow;
            else if (node.connect != null) GUI.color = Color.green;
            Rect r = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

            if (GUI.Button(r, ""))
            {
                selectNode = node;
                SelectNodeAction = (x) => node.connect = x;
            }
            GUI.color = temp;


            if (Event.current.type == EventType.Repaint) node.posConnect = node.myRect.position + r.position + new Vector2(10, 10);
            EditorGUILayout.EndHorizontal();
        }
        if (panningScreen) return;

        GUI.DragWindow();

        if (node.myRect.x < 0)
            node.myRect.x = 0;

        if (node.myRect.y < toolbarHeight)
            node.myRect.y = toolbarHeight;
    }

    void DrawRequirementNode(RequirementNode node)
    {
        if (node.minimized)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = false;
                node.myRect.height = node.expandRect.height;
                node.myRect.width = node.expandRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Expand");
            node.posConnectA = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                              node.myRect.position.y + node.myRect.height / 2f);
            node.posConnectB = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                              node.myRect.position.y + node.myRect.height / 2f);

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = true;
                node.myRect.height = node.minimizeRect.height;
                node.myRect.width = node.minimizeRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Minimize");

            EditorGUILayout.EndHorizontal();
            node.myName = EditorGUILayout.TextField("Node Name:", node.myName);
            node.ID = EditorGUILayout.IntField("ID:", node.ID);

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("True:", GUILayout.Width(145));
            Color temp = GUI.color;
            if (selectNode == node && node.selectedOption == 1) GUI.color = Color.yellow;
            else if (node.connectA != null) GUI.color = Color.green;
            Rect r = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

            if (GUI.Button(r, ""))
            {
                selectNode = node;
                SelectNodeAction = (x) => node.connectA = x;
                node.selectedOption = 1;
            }
            GUI.color = temp;


            if (Event.current.type == EventType.Repaint) node.posConnectA = node.myRect.position + r.position + new Vector2(10, 10);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("False:", GUILayout.Width(145));
            if (selectNode == node && node.selectedOption == 2) GUI.color = Color.yellow;
            else if (node.connectB != null) GUI.color = Color.green;
            Rect r2 = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

            if (GUI.Button(r2, ""))
            {
                selectNode = node;
                SelectNodeAction = (x) => node.connectB = x;
                node.selectedOption = 2;
            }
            GUI.color = temp;


            if (Event.current.type == EventType.Repaint) node.posConnectB = node.myRect.position + r2.position + new Vector2(10, 10);
            EditorGUILayout.EndHorizontal();
        }
        if (panningScreen) return;

        GUI.DragWindow();

        if (node.myRect.x < 0)
            node.myRect.x = 0;

        if (node.myRect.y < toolbarHeight)
            node.myRect.y = toolbarHeight;
    }

    void DrawDelayNode(DelayNode node)
    {
        if (node.minimized)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = false;
                node.myRect.height = node.expandRect.height;
                node.myRect.width = node.expandRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Expand");
            node.posConnect = new Vector2(node.myRect.position.x + node.myRect.width / 2f,
                              node.myRect.position.y + node.myRect.height / 2f);

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
            {
                node.minimized = true;
                node.myRect.height = node.minimizeRect.height;
                node.myRect.width = node.minimizeRect.width;
                Repaint();
                return;
            }
            EditorGUILayout.LabelField("Minimize");

            EditorGUILayout.EndHorizontal();
            node.delayTime = EditorGUILayout.FloatField("Delay:", node.delayTime);

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Connection:", GUILayout.Width(145));
            Color temp = GUI.color;
            if (selectNode == node) GUI.color = Color.yellow;
            else if (node.connect != null) GUI.color = Color.green;
            Rect r = new Rect(node.myRect.width - 30, GUILayoutUtility.GetLastRect().y, 20, 20);

            if (GUI.Button(r, ""))
            {
                selectNode = node;
                SelectNodeAction = (x) => node.connect = x;
            }
            GUI.color = temp;


            if (Event.current.type == EventType.Repaint) node.posConnect = node.myRect.position + r.position + new Vector2(10, 10);
            EditorGUILayout.EndHorizontal();
        }
        if (panningScreen) return;

        GUI.DragWindow();

        if (node.myRect.x < 0)
            node.myRect.x = 0;

        if (node.myRect.y < toolbarHeight)
            node.myRect.y = toolbarHeight;
    }
    #endregion
}

