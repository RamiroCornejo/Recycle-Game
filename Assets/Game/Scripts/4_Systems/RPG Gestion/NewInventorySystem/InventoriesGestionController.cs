using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum inventory_input_action { left_down, left_up, right_down, right_up, central_down, drag_begin, drag_end, ctrl_left_click, alt_left_click, mayus_left_click, drag_fail }

public class InventoriesGestionController : MonoBehaviour
{
    public static InventoriesGestionController instance;

    HashSet<MonoInvent_Base> openedInventories = new HashSet<MonoInvent_Base>();
    public MonoInvent_Base contextual_inventory;

    private void Awake()
    {
        instance = this;
    }

    public void OpenContextualInventory(MonoInvent_Base _contextual_inventory)
    {
        contextual_inventory = _contextual_inventory;
    }
    public void CloseContextualInventory()
    {
        contextual_inventory = null;
    }

    #region Open & Close Inventories
    
    public static void AddInventoryToOpened(MonoInvent_Base invent) => instance.AddInventoryToOpenedCollection(invent);
    public static void CloseAllInventories() => instance.CloseAllOpenedInventories();
    public void AddInventoryToOpenedCollection(MonoInvent_Base invent)
    {
        openedInventories.Add(invent);
    }
    public void CloseAllOpenedInventories()
    {
        foreach (var i in openedInventories) i.CloseInventory();
        openedInventories.Clear();
    }
    #endregion

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
        {
            CloseAllInventories();
        }

        if (Input.GetMouseButtonDown(1)) //click derecho
        {
            if (iHaveOneSelected)
            {
                memo_handler.Refresh();

                //le aviso al que tenia en memoria que se despinte
                memo_slot.DrawIsSelected(false);
                memo_handler = null;
                iHaveOneSelected = false;
            }
        }

        if (contextual_inventory != null)
        {
            for (int i = 0; i < PlayerInventory.instance.inventory.Container.Capacity; i++)
            {
                PlayerInventory.instance.inventory.Container.GetSlotByIndex(i);
            }
        }
    }

    bool iHaveOneSelected;
    public MonoInvet_Handler memo_handler; //pasar a privado cuando termine de debugear
    public UI_Slot memo_slot; //pasar a privado cuando termine de debugear

    bool leftpressed;
    public bool isDragging;
    public void SlotHasClicked(inventory_input_action action, MonoInvet_Handler clicked_handler, UI_Slot clicked_slot)
    {
       // Debug.Log(action);

        if (action == inventory_input_action.left_down)
        {
            leftpressed = true;
        }

        if (action == inventory_input_action.left_up) //click izquierdo
        {
            if (leftpressed && !isDragging)
            {
                leftpressed = false;

                if (iHaveOneSelected)
                {
                    int INDEX_MEMO = memo_slot.Slot.Position;
                    int INDEX_CLICKED = clicked_slot.Slot.Position;

                    string debslot = clicked_slot.Slot != null ? "[" + clicked_slot.Slot.Position + "] " + (clicked_slot.Slot.IsEmpty ? " ->EMPTY" : "->" + clicked_slot.Slot.Element.Element_Name) : "NULL";
                    if (DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory(debslot, clicked_handler.StorageName);

                    //estos son los filtros de objeto, si lo que clikeas tiene un filtro que cumplir y no cumple, cancelamos todo
                    //por ejemplo, un tacho para almacenar plastico no puede almacenar metal
                    if (clicked_handler.Check_filter(memo_handler.GetSlotByIndex(INDEX_MEMO).Stack))
                    {
                        Slot slot_clicked = clicked_handler.GetSlotByIndex(INDEX_CLICKED);

                        if (slot_clicked.IsEmpty)
                        {
                            // [ el casillero que toqué estaba vacio ]
                            // simplemente meto la informacion en el casillero vacio
                            // y limpio la que tenia en memoria
                            if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                            if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                            slot_clicked.OverrideStack(memo_handler.GetSlotByIndex(INDEX_MEMO).GetFullCopy());
                            memo_handler.GetSlotByIndex(INDEX_MEMO).Empty();

                            if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);

                        }
                        else
                        {
                           // Debug.Log("Tenia algo el clicked");
                            StackedPile memostack = memo_handler.GetSlotByIndex(INDEX_MEMO).GetFullCopy();
                            StackedPile currentstack = clicked_handler.GetSlotByIndex(INDEX_CLICKED).GetFullCopy();

                            bool current_dry = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.IsDry;
                            bool current_dirty = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.IsDirty;

                            if (!currentstack.Element.Equals(memostack.Element))
                            {
                                //switch

                                if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                                if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                clicked_handler.GetSlotByIndex(INDEX_CLICKED).OverrideStack(memostack);
                                memo_handler.GetSlotByIndex(INDEX_MEMO).OverrideStack(currentstack);

                                if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                                if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                            }
                            else
                            {
                                if (memostack.IsEqualByDebuffs(currentstack))
                                {
                                    if (INDEX_CLICKED != INDEX_MEMO)
                                    {
                                        if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                                        if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));


                                        //capturo
                                        int clicked = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.Quantity;
                                        int memory = memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.Quantity;
                                        int max = memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.MaxStack;

                                        //dropeo
                                        clicked.Drop(ref memory, max);

                                        //reasigno
                                        clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.Quantity = clicked;
                                        memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.Quantity = memory;

                                        if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                                        if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                    }
                                }
                                else
                                {
                                    if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                                    if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                    //switch
                                    clicked_handler.GetSlotByIndex(INDEX_CLICKED).OverrideStack(memostack);
                                    memo_handler.GetSlotByIndex(INDEX_MEMO).OverrideStack(currentstack);

                                    if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                                    if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                }
                            }
                        }
                    }
                    else
                    {
                        //no respeta el filtro
                        UIManager.instance.SendText("No puedo poner eso ahi");
                        SoundFX.Play_Dialogue_Negate();
                    }

                    clicked_handler.Refresh();
                    memo_handler.Refresh();

                    //le aviso al que tenia en memoria que se despinte
                    memo_slot.DrawIsSelected(false);
                    memo_handler = null;
                    iHaveOneSelected = false;
                }
                else
                {

                    if (DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory("", "");

                    if (clicked_slot.IsEmpty) return;

                    if (DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory("waiting", "waiting");

                    //le aviso al handler que se seleccione y se pinte
                    clicked_slot.DrawIsSelected(true);
                    memo_slot = clicked_slot;
                    memo_handler = clicked_handler;
                    iHaveOneSelected = true;
                }
            }
        }

        if (action == inventory_input_action.right_down) //click derecho
        {
            if (leftpressed || isDragging)
            {
                leftpressed = false;
                isDragging = false;

                if (iHaveOneSelected)
                {
                    clicked_handler.Refresh();
                    memo_handler.Refresh();

                    //le aviso al que tenia en memoria que se despinte
                    memo_slot.DrawIsSelected(false);
                    memo_handler = null;
                    iHaveOneSelected = false;
                }
            }
        }

        if (action == inventory_input_action.central_down) //click central
        {

        }

        if (action == inventory_input_action.drag_begin) //begin drag
        {
            if (iHaveOneSelected)
            {
                clicked_handler.Refresh();
                memo_handler.Refresh();

                //le aviso al que tenia en memoria que se despinte
                memo_slot.DrawIsSelected(false);
                memo_handler = null;
                memo_slot = null;
                iHaveOneSelected = false;
            }

            if (clicked_slot.IsEmpty)
            {
                return;
            }

            //Debug.Log("Inicio drag en: " + clicked_slot.Element.Element_Name);

            //le aviso al handler que se seleccione y se pinte
            clicked_slot.DrawIsSelected(true);
            memo_slot = clicked_slot;
            memo_handler = clicked_handler;
            iHaveOneSelected = true;

            if (DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory("waiting", "waiting");
        }

        if (action == inventory_input_action.drag_end) //end drag
        {
            isDragging = false;
            leftpressed = false;

            if (iHaveOneSelected)
            {
                int INDEX_MEMO = memo_slot.Slot.Position;
                int INDEX_CLICKED = clicked_slot.Slot.Position;

                string debslot = clicked_slot.Slot != null ? "[" + clicked_slot.Slot.Position + "] " + (clicked_slot.Slot.IsEmpty ? " ->EMPTY": "->" + clicked_slot.Slot.Element.Element_Name) : "NULL";
                if (DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory(debslot, clicked_handler.StorageName);

                //estos son los filtros de objeto, si lo que clikeas tiene un filtro que cumplir y no cumple, cancelamos todo
                //por ejemplo, un tacho para almacenar plastico no puede almacenar metal
                if (clicked_handler.Check_filter(memo_handler.GetSlotByIndex(INDEX_MEMO).Stack))
                {
                    Slot slot_clicked = clicked_handler.GetSlotByIndex(INDEX_CLICKED);

                    if (slot_clicked.IsEmpty)
                    {
                        // [ el casillero que toqué estaba vacio ]
                        // simplemente meto la informacion en el casillero vacio
                        // y limpio la que tenia en memoria
                        if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                        if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                        slot_clicked.OverrideStack(memo_handler.GetSlotByIndex(INDEX_MEMO).GetFullCopy());
                        memo_handler.GetSlotByIndex(INDEX_MEMO).Empty();

                        if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);

                    }
                    else
                    {
                        //Debug.Log("Tenia algo el clicked");
                        StackedPile memostack = memo_handler.GetSlotByIndex(INDEX_MEMO).GetFullCopy();
                        StackedPile currentstack = clicked_handler.GetSlotByIndex(INDEX_CLICKED).GetFullCopy();

                        bool current_dry = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.IsDry;
                        bool current_dirty = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.IsDirty;

                        if (!currentstack.Element.Equals(memostack.Element))
                        {
                            //switch

                            if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                            if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                            clicked_handler.GetSlotByIndex(INDEX_CLICKED).OverrideStack(memostack);
                            memo_handler.GetSlotByIndex(INDEX_MEMO).OverrideStack(currentstack);

                            if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                            if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                        }
                        else
                        {
                            if (memostack.IsEqualByDebuffs(currentstack))
                            {
                                if (INDEX_CLICKED != INDEX_MEMO)
                                {
                                    if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                                    if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));


                                    //capturo
                                    int clicked = clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.Quantity;
                                    int memory = memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.Quantity;
                                    int max = memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.MaxStack;

                                    //dropeo
                                    clicked.Drop(ref memory, max);

                                    //reasigno
                                    clicked_handler.GetSlotByIndex(INDEX_CLICKED).Stack.Quantity = clicked;
                                    memo_handler.GetSlotByIndex(INDEX_MEMO).Stack.Quantity = memory;

                                    if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                                    if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                }
                            }
                            else
                            {
                                if (!slot_clicked.IsEmpty) clicked_handler.Container.RemoveToRegister(slot_clicked.Element, slot_clicked);
                                if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.RemoveToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                                //switch
                                clicked_handler.GetSlotByIndex(INDEX_CLICKED).OverrideStack(memostack);
                                memo_handler.GetSlotByIndex(INDEX_MEMO).OverrideStack(currentstack);

                                if (!slot_clicked.IsEmpty) clicked_handler.Container.AddToRegister(slot_clicked.Element, slot_clicked);
                                if (!memo_handler.GetSlotByIndex(INDEX_MEMO).IsEmpty) memo_handler.Container.AddToRegister(memo_handler.GetSlotByIndex(INDEX_MEMO).Element, memo_handler.GetSlotByIndex(INDEX_MEMO));

                            }
                        }
                    }
                }
                else
                {
                    Negate();
                }

                clicked_handler.Refresh();
                memo_handler.Refresh();

                CleanMemory();
            }
            else
            {
                //selecciono uno

                if (clicked_slot.IsEmpty) return;

                //le aviso al handler que se seleccione y se pinte
                clicked_slot.DrawIsSelected(true);
                memo_slot = clicked_slot;
                memo_handler = clicked_handler;
                iHaveOneSelected = true;

                if(DebugContainerRegister.instance) DebugContainerRegister.instance.UpdateSlotAndInventory("waiting", "waiting");
            }
        }

        if (action == inventory_input_action.ctrl_left_click)
        {
            isDragging = false;
            leftpressed = false;
            if (iHaveOneSelected)
            {
                clicked_handler.Refresh();
                memo_handler.Refresh();
                memo_slot.DrawIsSelected(false);
                memo_handler = null;
                iHaveOneSelected = false;
            }
            if (clicked_slot.IsEmpty) return;

            if (contextual_inventory)
            {
                MonoInvent_Base destiny;
                MonoInvent_Base origin;

                if (clicked_handler.StorageName == contextual_inventory.StorageName)
                {
                    origin = contextual_inventory;
                    destiny = PlayerInventory.instance.inventory;
                }
                else
                {
                    origin = PlayerInventory.instance.inventory;
                    destiny = contextual_inventory;
                }

                origin.Container.RemoveToRegister(clicked_slot.Slot.Element, clicked_slot.Slot);

                var dest = (MonoInvet_Handler)destiny;

                if (dest.Check_filter(clicked_slot.Slot.Stack))
                {
                    ElementData element = clicked_slot.Element;
                    int quantity = clicked_slot.Slot.Quantity;
                    int quality = clicked_slot.Slot.Stack.Quality;
                    bool dirty = clicked_slot.Slot.Stack.IsDirty;
                    bool dry = clicked_slot.Slot.Stack.IsDry;

                    var result = dest.AddElement(element, quantity, quality, dry, dirty);

                    if (!result.Process_Successfull)
                    {
                        int remainder = result.Remainder_Quantity;
                        clicked_slot.Slot.Stack.ModifyQuantity(remainder);
                    }
                    else
                    {
                        //Debug.Log("estoy removiendo todo");
                        //origin.Container.RemoveToRegister(element, );
                        origin.RemoveElement(element, quantity, quality, dry, dirty, true, clicked_slot.Slot.Position);
                        
                    }

                    origin.Refresh();
                    dest.Refresh();
                    clicked_slot.DrawIsSelected(false);
                    clicked_handler = null;
                    clicked_slot = null;
                    iHaveOneSelected = false;
                }
            }
        }

    }

    void Negate()
    {
        UIManager.instance.SendText("No puedo poner eso ahi");
        SoundFX.Play_Dialogue_Negate();
    }

    void CleanMemory()
    {
        isDragging = false;
        leftpressed = false;
        if (memo_slot) memo_slot.DrawIsSelected(false);
        memo_handler = null;
        memo_slot = null;
        iHaveOneSelected = false;
    }

    public void DropFail()
    {
        leftpressed = false;
        isDragging = false;

        if (iHaveOneSelected)
        {
            memo_handler.Refresh();
            if (memo_slot) memo_slot.DrawIsSelected(false);
            memo_handler = null;
            iHaveOneSelected = false;
        }
    }
}
