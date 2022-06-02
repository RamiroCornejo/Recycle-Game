using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedDialogue : ScriptableObject
{
    public NodeSave[] allNodes;
    public NodeSave firstNode;

    public bool nextDialogue = false;

    public bool repeatDialogue = false;

    public NodeSave SearchNodeByID(int ID)
    {
        for (int i = 0; i < allNodes.Length; i++)
        {
            if (ID == allNodes[i].ID)
                return allNodes[i];
        }

        Debug.LogError("No hay ningún nodo con ese ID");
        return null;
    }
}
