using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalContainer : InventoryBase
{
    bool opened;
    public void ExecuteOpen()
    {
        if (opened) return;
        //Debug.Log("Execute Open");
        opened = true;
        SubscribeToClose(CloseByExternals);
        Open(true);
        
    }
    void CloseByExternals()
    {
        opened = false;

    }

    public void ExecuteExit()
    {
        if (!opened) return;
        //Debug.Log("Exit To Far");
        opened = false;
        Close();
    }

    protected override void Start()
    {
        base.Start();
    }
}
