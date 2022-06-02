using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public abstract class MonoInvent_Base : MonoBehaviour
{

    [SerializeField] protected string storage_name;
    public string StorageName => storage_name;

    [SerializeField] protected Container container;
    public Container DEBUG_Container => container;
    public Container Container => container;

    Action OnCloseInventoryCallback = delegate { };

    public HashSet<int> positions_to_enable = new HashSet<int>();

    //si nuestro ivnentario siempre va a estar en pantalla y nunca vamos a cerrarlo activar este flag
    public bool isStaticInventory = false;

    public void Build(int _capacity)
    {
        container = new Container(_capacity);
        OnBuild();
    }

    bool isOpen;
    public bool IsOpen => isOpen;

    public void PrintMessage(string _msg) => OnMessage(_msg);

    public Slot GetSlotByIndex(int index_position) => container.GetSlotByIndex(index_position);

    public CPResult AddElement(ElementData data, int quantity, int quality, bool _isDry, bool _isDirty)
    {
        var result = container.Add_Element(data, quantity, quality, _isDry, _isDirty);
        OnElementAdded(result);
        return result;
    }
    public CPResult RemoveElement(ElementData data, int quantity, int quality, bool isDry, bool isDirty, bool respectQuality = false, int specific_index = -1)
    {
        var result = container.Remove_Element(data, quantity, quality, isDry, isDirty, respectQuality, specific_index);
        OnElementRemoved(result);
        return result;
    }

    public void AddSlotsInRuntime(int quant_slots_to_add)
    {
        container.AddNewSlots(quant_slots_to_add);
        Refresh(true);
    }

    public void Refresh(bool redraw = false)
    {
        OnRefresh(redraw);
    }

    public void OpenInventory(bool UsetrackInput = true)
    {
        if (isOpen) return;
        if (!isStaticInventory) InventoriesGestionController.instance.OpenContextualInventory(this);
        if (!isStaticInventory) PlayerInventory.OpenVisualsByContext();
        if (!isStaticInventory) InventoriesGestionController.AddInventoryToOpened(this);
        if (!isStaticInventory) isOpen = true;
        OnOpenInventory();
        if (!isStaticInventory) if (UsetrackInput) Character.TrackInput(false);
        UI_ContainerManager.instance_playerContainer.Refresh();
        
        
    }
    public void SubscribeToCloseInventory(Action OnClose)
    {
        OnCloseInventoryCallback = OnClose;
    }
    public void CloseInventory()
    {
        if (!isOpen) return;
        if (!isStaticInventory) PlayerInventory.CloseVisualsByContext();
        if (!isStaticInventory) InventoriesGestionController.instance.CloseContextualInventory();
        if (!isStaticInventory) isOpen = false;
        //Debug.Log("Se esta cerrando");
        OnCloseInventory();
        OnCloseInventoryCallback.Invoke();
        if (!isStaticInventory) Character.TrackInput(true);

        UI_ContainerManager.instance_playerContainer.Refresh();
    }

    protected abstract void OnMessage(string msg);
    protected abstract void OnBuild();
    protected abstract void OnElementRemoved(CPResult result);
    protected abstract void OnElementAdded(CPResult result);

    protected abstract void OnOpenInventory();
    protected abstract void OnCloseInventory();
    protected abstract void OnRefresh(bool redraw);

    public bool QueryElement(ElementData data, int quantity, int quality, bool _isDry, bool _isDirty, bool respectQuality = false)
    {
        return container.QueryElement(data, quantity, quality, _isDry, _isDirty, respectQuality);
    }

}
