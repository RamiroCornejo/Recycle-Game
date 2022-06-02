using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReq_ObjCheck : QuestRequirement
{
    [SerializeField] ObjectData[] requireObjects = new ObjectData[0];
    [SerializeField] NPCRequestUI ui = null;

    bool dontShowUI = false;
    string[] myItemsOnString;

    private void Start()
    {
        myItemsOnString = new string[requireObjects.Length];

        for (int i = 0; i < requireObjects.Length; i++)
        {
            string plural = requireObjects[i].ammount > 1 ? requireObjects[i].element.Plural_Name : requireObjects[i].element.Element_Name;

            myItemsOnString[i] = requireObjects[i].ammount + " " + plural;
        }

        ui?.Initialize(requireObjects);
    }

    public void QuestUIShow()
    {
        if (dontShowUI) return;
        ui?.Show();
    }

    public void QuestUIClose()
    {
        if (dontShowUI) return;

        ui?.Close();
    }

    public override bool IsRequireComplete()
    {
        for (int i = 0; i < requireObjects.Length; i++)
        {
            if (!PlayerInventory.QueryElement(requireObjects[i].element, requireObjects[i].ammount, 1, requireObjects[i].isDry, requireObjects[i].isDirty, true))
                return false;
        }

        return true;
    }

    public override void RequireRemove(bool isOneShot)
    {
        for (int i = 0; i < requireObjects.Length; i++)
        {
            PlayerInventory.Remove(requireObjects[i].element, requireObjects[i].ammount, 1, requireObjects[i].isDry, requireObjects[i].isDirty, true);
        }

        QuestUIClose();

        dontShowUI = isOneShot;
    }


    public string[] GetItemsOnString()
    {
        return myItemsOnString;


    }
}

[System.Serializable]
public struct ObjectData
{
    public ElementData element;
    public int ammount;
    public bool isDry;
    public bool isDirty;
}
