using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugContainerRegister : MonoBehaviour
{
    public TextMeshProUGUI container;
    public TextMeshProUGUI register;

    public MonoInvent_Base inventory_to_DEBUG;

    public static DebugContainerRegister instance;
    private void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI inventorymemo;
    public TextMeshProUGUI inventoryclicked;
    public TextMeshProUGUI slotmemo;
    public TextMeshProUGUI slotclicked;

    // Update is called once per frame
    void Update()
    {
        container.text = inventory_to_DEBUG.DEBUG_Container.UpdateDebug_Slots();
        register.text = inventory_to_DEBUG.DEBUG_Container.UpdateDebug_Registers();

        var memoHand = InventoriesGestionController.instance.memo_handler;
        var memoSlot = InventoriesGestionController.instance.memo_slot;

        inventorymemo.text = memoHand != null ? memoHand.StorageName : " NULL" ;
        slotmemo.text = memoSlot != null ? "[" + memoSlot.Slot.Position + "] "+ (memoSlot.Slot.IsEmpty ? "->Empty" : memoSlot.Slot.Element.Element_Name) : "NULL";
    }
    public void UpdateSlotAndInventory(string slot, string inventory)
    {
        inventoryclicked.text = inventory;
        slotclicked.text = slot;
    }
}
