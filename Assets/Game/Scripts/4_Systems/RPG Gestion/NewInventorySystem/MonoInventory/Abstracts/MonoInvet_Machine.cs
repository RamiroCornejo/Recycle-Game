using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonoInvet_Machine : MonoInvet_Handler
{
    public ProgressFill myProgress;
    public string machine_description;

    MachineAnimations myAnimations;

    protected override void Start()
    {
        base.Start();
        myProgress = GetComponentInChildren<ProgressFill>();
        myProgress.Close();
        myAnimations = GetComponentInChildren<MachineAnimations>();
        if (myAnimations)
        {
           // Debug.Log("Animation in" + gameObject.name);
            myAnimations.SubscribeToSpitEvent(ANIM_EVENT_SpitEvent);
        }
    }

    

    protected override void OnBuild() { }
    protected override void OnElementAdded(CPResult result)
    {
        base.OnElementAdded(result);
        CheckToProcess();
    }
    protected override void OnElementRemoved(CPResult result)
    {
        base.OnElementRemoved(result);
        CheckToProcess();
    }

    protected override void OnMessage(string msg) { }
    protected override void OnOpenInventory()
    {
        base.OnOpenInventory();
        UI_ChestMachine.instance.Redraw(this, container, true);
        UI_ChestMachine.instance.OpenChestMachine(storage_name, OnFinishFadeAnimation_Open);
        UI_ChestMachine.instance.SetDescription(machine_description);
        MonoInvet_Chest.OpenGlobalChest();
        UI_ChestMachine.instance.EnableProgressBar(false);
    }
    protected override void OnCloseInventory()
    {
        base.OnCloseInventory();
        UI_ChestMachine.instance.CloseChestMachine(OnFinishFadeAnimation_Close);
        UI_ChestMachine.instance.ProgressProcess(0);
        MonoInvet_Chest.CloseGlobalChest();

        UI_ChestMachine.instance.TaskValue("Esperando objetos...");
        UI_ChestMachine.instance.UpdateElementObject(UI_InventoryDataBase.SlotSpecial);
        UI_ChestMachine.instance.ProgressProcess(0);
        UI_ChestMachine.instance.EnableProgressBar(false);
    }
    protected override void OnRefresh(bool redraw = false)
    {
        base.OnRefresh(redraw);
        UI_ChestMachine.instance.Refresh(redraw);
        CheckToProcess();
    }

    StackedPile process_element;
    bool begin_process;
    public float timer;
    public float time_to_process = 1;
    bool isProcessing;
    public void CheckToProcess()
    {
        if (isProcessing) return;

        for (int i = 0; i < container.Capacity; i++)
        {
            Slot slot = container.GetSlotByIndex(i);
            if (slot.IsEmpty) continue;
            if (CoditionToProcess(slot.Stack))
            {
                if(myAnimations) myAnimations.PlayProcess();
                //copio y remuevo
                process_element = slot.Stack.ManualCopy();
                var result = container.Remove_Element(slot.Element, 1, slot.Stack.Quality, slot.Stack.IsDry, slot.Stack.IsDirty, true);

                if (!result.Process_Successfull) throw new System.Exception("Error rarisimo, deberia poder remover algo que tengo");
                if (IsOpen) UI_ChestMachine.instance.UpdateElementObject(process_element.Element.Element_Image);
                isProcessing = true;
                begin_process = true;
                if (IsOpen) UI_ChestMachine.instance.Refresh(false);
                if (IsOpen) UI_ChestMachine.instance.TaskValue("Procesando...");
                myProgress.Open();
                return;
            }
        }

        if (myAnimations) myAnimations.StopProcess();
        if (IsOpen) UI_ChestMachine.instance.TaskValue("Esperando objetos...");
        if (IsOpen) UI_ChestMachine.instance.UpdateElementObject(UI_InventoryDataBase.SlotSpecial);
        if (IsOpen) UI_ChestMachine.instance.ProgressProcess(0);
        if (IsOpen) UI_ChestMachine.instance.EnableProgressBar(false);
        myProgress.Close();
        myProgress.Fill(0);
        begin_process = false;
        timer = 0;
    }

    float restoprogress;
    public void Update()
    {
        if (begin_process)
        {
            if (IsOpen) UI_ChestMachine.instance.EnableProgressBar(true);

            if (timer < time_to_process)
            {
                timer = timer + 1 * Time.deltaTime;
                restoprogress = timer / time_to_process;

                if(IsOpen) UI_ChestMachine.instance.ProgressProcess(restoprogress);
                myProgress.Fill(restoprogress);
            }
            else
            {
                timer = 0;

                SucessffulProcess();
                isProcessing = false;
                CheckToProcess();
                if (IsOpen) UI_ChestMachine.instance.EnableProgressBar(false);
            }
        }
    }

    void SucessffulProcess()
    {
        //Debug.Log("Sucessfull");
        if (myAnimations) myAnimations.SpitAnim();
        else
        {
            TreatElement(process_element);
        }
    }

    protected virtual void ANIM_EVENT_SpitEvent()
    {
        TreatElement(process_element);
    }

    public abstract void TreatElement(StackedPile stack);
    public abstract bool CoditionToProcess(StackedPile stack);

    public abstract void OnFinishFadeAnimation_Open();
    public abstract void OnFinishFadeAnimation_Close();
}
