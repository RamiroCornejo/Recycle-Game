using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_ExitAllInventories : MonoBehaviour
{
    public void Exit()
    {
        InventoriesGestionController.instance.CloseAllOpenedInventories();
    }
}
