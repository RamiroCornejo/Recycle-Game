using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayerInventory : InventoryBase
{
    public static PlayerInventory instance;
    private void Awake() => instance = this;
    public UI_ContainerManager myContainer; 

    protected override void Start()
    {
        base.Start();

        OpenInventory();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    AddSlots(3);
        //}
    }

    public static void OpenVisualsByContext()
    {
        instance.OpenByContext();
    }
    void OpenByContext()
    {
        //var context = InventoriesGestionController.instance.contextual_inventory;
        //HashSet<int> to_select_view = new HashSet<int>();

        //try
        //{
        //    var chest = (MonoInvet_Chest)context;
        //    if (chest != null)
        //    {
        //        for (int i = 0; i < inventory.Container.Capacity; i++)
        //        {
        //            var filter = chest.GetComponent<FilterRecycled>();
        //            if (filter != null)
        //            {
        //                if (inventory.Container.GetSlotByIndex(i).IsEmpty) continue;

        //                if (filter.Check(inventory.Container.GetSlotByIndex(i).Stack))
        //                {
        //                    //
        //                    int position = inventory.Container.GetSlotByIndex(i).Position;
        //                    to_select_view.Add(position);
        //                }
        //            }
        //        }
        //        myContainer.SetCanInteract(to_select_view);
        //        return;
        //    }
        //}
        //catch (System.InvalidCastException) { }

        //try
        //{
        //    var machine = (MonoInvent_SimpleMachine)context;
        //    if (machine != null)
        //    {
        //        for (int i = 0; i < inventory.Container.Capacity; i++)
        //        {
        //            if (inventory.Container.GetSlotByIndex(i).IsEmpty) continue;

        //            if (machine.Check_filter(inventory.Container.GetSlotByIndex(i).Stack))
        //            {
        //                int position = inventory.Container.GetSlotByIndex(i).Position;
        //                to_select_view.Add(position);
        //            }
        //        }
        //        myContainer.SetCanInteract(to_select_view);
        //        return;
        //    }
        //}
        //catch (System.InvalidCastException) { }
        
    }

    public static void CloseVisualsByContext()
    {
        //InventoriesGestionController.instance.CloseContextualInventory();
        //instance.inventory.positions_to_enable.Clear();
        //instance.myContainer.SetCanInteract(new HashSet<int>());
    }

    public static void AddSlots(int quantToAdd)
    {
        instance.inventory.AddSlotsInRuntime(quantToAdd);
    }
    // PUBLIC STATICS
    public static Slot GetSlotByIndexPosition(int slot_position) => instance.inventory.GetSlotByIndex(slot_position);
    public static void Remove(ElementData elem, int quant, int quality, bool isDry, bool isDirty, bool respectQuality = false) => instance.Remove_To_Inventory(elem, quant, quality, isDry, isDirty, respectQuality);
    public static CPResult Add(ElementData elem, int quant, int quality, bool _isDry, bool _isDirty)
    {
        var result = instance.Add_To_Inventory(elem, quant, quality, _isDry, _isDirty);

        if (result.Process_Successfull) UIManager.instance.AddItem(new ItemValues(elem.Element_Image, _isDry, _isDirty));
        return result; }
    public static bool QueryElement(ElementData elem, int quant, int quality, bool _isDry, bool _isDirty, bool respectQuality) => instance.Query_Element(elem, quant, quality, _isDry, _isDirty, respectQuality);
    public static void OpenInventory() => instance.Open(true, false);
    public static void CloseInventory() => instance.Close();


}

