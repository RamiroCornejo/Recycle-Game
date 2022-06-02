using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_ChestBin : MonoBehaviour
{
    public static UI_ChestBin instance;
    private void Awake() => instance = this;

    [SerializeField] UI_ContainerManager myUI;
    public CanvasGroup myGroup;

    public void Redraw(MonoInvet_Handler handler,Container container, bool redraw = false)
    {
        myUI.Build(handler, container, redraw);
    }

    public void Refresh(bool redraw = false)
    {
        myUI.Refresh(redraw);
    }

    public void OpenChestBin(string storage_name, Action OnFinishFade)
    {

        myGroup.alpha = 1;
        myGroup.blocksRaycasts = true;
        myGroup.interactable = true;
        myUI.SetName(storage_name);
        myUI.OpenUIs(OnFinishFade);
    }
    public void CloseChestBin(Action OnFinishFade)
    {
        Debug.Log("CloseBIN");
        myGroup.alpha = 0;
        myGroup.blocksRaycasts = false;
        myGroup.interactable = false;
        myUI.CloseUIs(OnFinishFade);
    }
}
