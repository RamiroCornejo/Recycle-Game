using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class UI_ContainerManager : UI_ContainerBase
{
    [SerializeField] RectTransform myParent;

    [SerializeField] UI_Slot[] mySlots;
    HashSet<int> positions_can_Interact = new HashSet<int>();

    MonoInvet_Handler myHandler;
    public static UI_ContainerManager instance_playerContainer;

    [SerializeField] TextMeshProUGUI storage_name;
    [SerializeField] UIFade fade;

    public bool isPlayerInventory;

    protected override void Start()
    {
        base.Start();
        if (isPlayerInventory) instance_playerContainer = this;
    }

    public void ElementAction(inventory_input_action action, UI_Slot slot)
    {
        //if (action == -1) PlayerInventory.instance.Equip(this);
        //if (action == -2) PlayerInventory.instance.Use(this);
        //if (action == -3) PlayerInventory.instance.Cancel(this);

        InventoriesGestionController.instance.SlotHasClicked(action, myHandler, slot);
    }

    public void SetCanInteract(HashSet<int> slots_can_interact)
    {
        positions_can_Interact = slots_can_interact;
        Debug.Log("Tengo " + slots_can_interact.Count + " posiciones interactuables");
    }

    public void OpenUIs(Action OnFinishAnimation)
    {
        if (fade) fade.FadeIn(OnFinishAnimation);
    }
    public void CloseUIs(Action OnFinishAnimation)
    {
        if (fade) fade.FadeOut(OnFinishAnimation);
    }

    public void SetName(string storagename)
    {
        storage_name.text = storagename;
    }

    public void SelectFirst()
    {
        mySlots[0].Select();
    }

    int currentCapacity;

    public void Build(MonoInvet_Handler handler, Container container, bool redraw = false)
    {
        myHandler = handler;

        int cap = container.Capacity;

        if (redraw)
        {
            // Debug.Log("Estoy redibujando");
            for (int i = 0; i < mySlots.Length; i++)
            {
                Destroy(mySlots[i].gameObject);
            }
        }

        mySlots = new UI_Slot[cap];

        for (int i = 0; i < cap; i++)
        {
            var slot = GameObject.Instantiate(UI_InventoryDataBase.SlotUIModel, myParent);
            slot.ConfigureSlot(container.GetSlotByIndex(i), ElementAction);
            slot.Refresh();

            mySlots[i] = slot;
        }

        currentCapacity = container.Capacity;
    }

    public void Refresh(bool redraw = false)
    {
        if (myHandler.Container.Capacity > currentCapacity)
        {
            Build(myHandler, myHandler.Container, redraw);
        }
        foreach (var s in mySlots)
        {
            s.Refresh();

            if (!isPlayerInventory) continue;
            if (!InventoriesGestionController.instance.contextual_inventory) { s.NormalInteraction(); continue; }
            if (s.Slot.IsEmpty) continue;

            var context = InventoriesGestionController.instance.contextual_inventory;

            try
            {
                var chest = (MonoInvet_Chest)context;
                var filter = chest.GetComponent<FilterRecycled>();
                if (filter != null)
                {
                    s.EnableInteraction(filter.Check(s.Slot.Stack));
                }
            }
            catch (System.InvalidCastException) { }

            try
            {
                var machine = (MonoInvent_SimpleMachine)context;
                s.EnableInteraction(machine.CoditionToProcess(s.Slot.Stack));
            }
            catch (System.InvalidCastException) { }


        }
    }
}
