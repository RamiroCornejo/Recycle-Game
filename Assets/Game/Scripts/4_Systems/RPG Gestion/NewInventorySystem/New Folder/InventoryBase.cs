using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using System;

public class InventoryBase : MonoBehaviour
{
    public int capacity = 10;
    public MonoInvet_Handler inventory;
    
    protected bool isOpen = true;

    protected const bool FORCE_USE = true;

    protected virtual void Start()
    {
        inventory.Build(capacity);
    }

    #region Basic Handlers

    protected CPResult Add_To_Inventory(ElementData elem, int quant, int quality,bool _isDry, bool _isDirty) => 
        inventory.AddElement(elem, quant, quality, _isDry, _isDirty);

    protected CPResult Remove_To_Inventory(ElementData elem, int quant, int quality, bool isDry, bool isDirty, bool respectQuality = false) => 
        inventory.RemoveElement(elem, quant, quality, isDry, isDirty, respectQuality);

    protected bool Query_Element(ElementData elem, int quant, int quality, bool _isDry, bool _isDirty, bool respectQuality) =>
        inventory.QueryElement(elem, quant, quality, _isDry, _isDirty, respectQuality);


    #endregion

    #region Selection
    public void Select(UI_Slot ui_slot)
    {
        if (ui_slot.Element)
        {
            inventory.SendMessage("Select: " + ui_slot);
        }
        else
        {
            inventory.SendMessage("Select: VACIO");
        }
    }
    public void Cancel(UI_Slot ui_slot) => SendMessage(ui_slot.Element ? "Cancel: " + ui_slot : "Cancel: VACIO");
    #endregion

    #region Open & Close
    public void SubscribeToClose(Action CallbackClose)
    {
        inventory.SubscribeToCloseInventory(CallbackClose);
    }
    public void Open(bool select, bool UsetrackInput = true)
    {
        inventory.OpenInventory(UsetrackInput);
        InputManagerShortcuts.OpenMenues();
    }
    public void Close()
    {
        inventory.CloseInventory();
        InputManagerShortcuts.CloseMenues();
    }
    #endregion
}
