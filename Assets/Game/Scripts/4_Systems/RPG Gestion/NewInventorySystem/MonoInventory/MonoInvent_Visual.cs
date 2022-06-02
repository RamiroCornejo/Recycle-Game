using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonoInvent_Visual : MonoInvet_Handler
{
    [SerializeField] CanvasGroup canvasGroup;
    public UI_ContainerManager ui_manager;
    public TextMeshProUGUI result_message;

    protected override void OnBuild()
    {
        base.OnBuild();

        ui_manager.Build(this,container);
    }

    protected override void OnElementRemoved(CPResult result)
    {
        base.OnElementRemoved(result);

        result_message.color = result.Process_Successfull ? Color.green : Color.red;
        result_message.text = "[" + result.Remainder_Quantity + "] " + result.Message;

        ui_manager.Refresh(false);
    }

    protected override void OnElementAdded(CPResult result)
    {
        base.OnElementAdded(result);

        result_message.color = result.Process_Successfull ? Color.green : Color.red;
        result_message.text = "[" + result.Remainder_Quantity + "] " + result.Message;

        ui_manager.Refresh(false);
    }

    protected override void OnMessage(string msg)
    {
        base.OnMessage(msg);
        result_message.text = msg;
    }

    protected override void OnOpenInventory()
    {
        base.OnOpenInventory();
        canvasGroup.alpha = 1;
     //   canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ui_manager.SelectFirst();
    }

    protected override void OnCloseInventory()
    {
        base.OnCloseInventory();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    protected override void OnRefresh(bool redraw = false)
    {
        base.OnRefresh(redraw);
        ui_manager.Refresh(redraw);
    }


}
