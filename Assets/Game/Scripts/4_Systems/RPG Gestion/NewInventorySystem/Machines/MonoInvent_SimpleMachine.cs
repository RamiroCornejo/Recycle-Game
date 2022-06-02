using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum  typemachine { lavadora, secadora, procesadora, crafter }
public class MonoInvent_SimpleMachine : MonoInvet_Machine
{
    public Transform spawnpoint;
    public typemachine _typemachine;

    #region Procesadora variables
    [Header("Procesadora")]
    [SerializeField] ConditionalElementComponent conditionalComponent;
    [SerializeField] string nameGeneralTask;
    [SerializeField] int current = 0;
    [SerializeField] int max = 5;
    [SerializeField] ElementData toDrop;
    [SerializeField] ProgressFill general_task_progress;
    #endregion

    #region Crafter variables
    [Header("Crafter")]
    public CraftingComponent craftingComponent;

    #endregion

    protected override void Start()
    {
        base.Start();
        if (_typemachine == typemachine.procesadora)
        {
            conditionalComponent = GetComponent<ConditionalElementComponent>();
            general_task_progress.Close();
            
        }    
    }

    protected override void OnOpenInventory()
    {
        base.OnOpenInventory();
        if (_typemachine == typemachine.procesadora)
        {
            UI_ChestMachine.instance.OpenCounter();
            UI_ChestMachine.instance.SetCounter(current, max, 0);
            UI_ChestMachine.instance.SetCounterNameTask(nameGeneralTask);
        }
    }
    protected override void OnCloseInventory()
    {
        base.OnCloseInventory();
        if (_typemachine == typemachine.procesadora)
        {
            UI_ChestMachine.instance.CloseCounter();
        }
    }

    public override bool CoditionToProcess(StackedPile elem)
    {
        if (_typemachine == typemachine.lavadora)
        {
            return true;
        }
        else if (_typemachine == typemachine.secadora)
        {
            return !elem.IsDry;
        }
        else if (_typemachine == typemachine.procesadora)
        {
            return conditionalComponent.Check(elem);
        }
        else
        {
            return craftingComponent.QueryElement(elem);
        }
    }

    public override void TreatElement(StackedPile elem)
    {
        if (_typemachine == typemachine.procesadora)
        {
            general_task_progress.Open();

            current++;

            if (current >= max)
            {
                current = 0;
                var item = GameManager.instance.GetItem(spawnpoint.position);
                item.SetDataDryAndClean(toDrop, false);
                item.SpitOnDirection(spawnpoint.forward);
                SoundFX.Play_Machine_Generic_Spit(transform);
                general_task_progress.Close();
            }

            float progress = (float)current / (float)max;
            general_task_progress.Fill(progress);
            if (IsOpen) UI_ChestMachine.instance.SetCounter(current, max, progress);

        }
        else if (_typemachine == typemachine.crafter)
        {
            var elements = craftingComponent.GetOutputByInput(elem);

            foreach (var e in elements)
            {
                var item = GameManager.instance.GetItem(spawnpoint.position);
                item.SetDataDryAndClean(e, false);
                SoundFX.Play_Machine_Generic_Spit(transform);
                item.SpitOnDirection(spawnpoint.forward);
            }
        }
        else
        {
            var item = GameManager.instance.GetItem(spawnpoint.position);
            if (_typemachine == typemachine.lavadora)
            {
                item.SetData(elem.Element, false, false, false);
            }
            else
            {
                item.SetData(elem.Element, false, true, elem.IsDirty);
            }

            item.SpitOnDirection(spawnpoint.forward);
            SoundFX.Play_Machine_Generic_Spit(transform);
        }
    }

    public override void OnFinishFadeAnimation_Open()
    {
        if (_typemachine == typemachine.procesadora) conditionalComponent.PlayCustomSoundOnOpen();
        if (_typemachine == typemachine.lavadora) SoundFX.Play_Machine_Open_WashingMachine();
        if (_typemachine == typemachine.secadora) SoundFX.Play_Machine_Open_Dryer();
    }

    public override void OnFinishFadeAnimation_Close()
    {
        if (_typemachine == typemachine.procesadora) conditionalComponent.PlayCustomSoundOnClose();
    }
}
