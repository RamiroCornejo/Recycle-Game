using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInvet_Chest : MonoInvet_Handler
{

    public bool isGlobal;
    public static MonoInvet_Chest instanceGlobal;
    PhysicalContainer myPhysicalContainer;
    public ChestAnimation myanim;

    public void Awake()
    {
        if (isGlobal)
        {
            if (instanceGlobal == null)
            {
                instanceGlobal = this;
                myPhysicalContainer = GetComponent<PhysicalContainer>();
            }
            else
            {
                throw new System.Exception("fatal error: No pueden haber dos cofres globales");
            }

        }
    }

    protected override void Start()
    {
        base.Start();
        myanim.SubscribeToFinishOpen(OnFinishCanANimationOpen);
    }


    public static void OpenGlobalChest()
    {
        if (instanceGlobal)
        {
            instanceGlobal.OpenInventory();
            
        }
    }

    public static void CloseGlobalChest()
    {
        if (instanceGlobal)
        {
            instanceGlobal.CloseInventory();
            
        }
    }

    protected override void OnBuild() { }
    protected override void OnElementAdded(CPResult result) { }
    protected override void OnElementRemoved(CPResult result) { }
    protected override void OnMessage(string msg) { }
    protected override void OnOpenInventory()
    {
        base.OnOpenInventory();
        myanim.Open();
    }

    void OnFinishCanANimationOpen()
    {
        UI_ChestBin.instance.Redraw(this, container, true);
        UI_ChestBin.instance.OpenChestBin(storage_name, OnFinishFadeOpen);
    }


    void OnFinishFadeOpen()
    {
        //SoundFX.Play_Storage_Open();
    }

    protected override void OnCloseInventory()
    {
        base.OnCloseInventory();
        UI_ChestBin.instance.CloseChestBin(OnFinishAnimationClose);
    }
    void OnFinishAnimationClose()
    {
        //SoundFX.Play_Storage_Close();
        myanim.Close();
    }

    protected override void OnRefresh(bool redraw = false)
    {
        base.OnRefresh(redraw);
        UI_ChestBin.instance.Refresh(redraw);
    }
}
