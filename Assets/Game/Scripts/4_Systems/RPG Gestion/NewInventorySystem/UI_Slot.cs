using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_Slot : Selectable, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image myBkgSlot;
    [SerializeField] Image myElementImage;
    //[SerializeField] Image elementQuality;
    [SerializeField] TextMeshProUGUI myCant;
    [SerializeField] CanvasGroup content;

    [SerializeField] GameObject isDryGameObject;
    [SerializeField] GameObject isDirtyGameObject;

    RectTransform myRect;

    Slot slot;
    public ElementData Element => slot.Element ? slot.Element : null;
    public Slot Slot => slot;
    public bool IsEmpty => slot.IsEmpty;

    public Action<inventory_input_action, UI_Slot> SlotAction = delegate { };

    [SerializeField] GameObject Tab_Object;

    public void ConfigureSlot(Slot slot, Action<inventory_input_action, UI_Slot> SlotAction)
    {
        this.slot = slot;
        this.SlotAction = SlotAction;
        myRect = GetComponent<RectTransform>();
    }

    Vector3 normalscale = new Vector3(1f, 1f, 1f);
    Vector3 littlescale = new Vector3(1.3f, 1.3f, 1.3f);

    protected override void OnDestroy()
    {
        base.OnDestroy();
        //Debug.Log("Se esta Destruyendo");
    }

    public void DrawIsSelected(bool drawvalue)
    {
        Tab_Object.SetActive(drawvalue);
        myElementImage.transform.localScale = drawvalue ? littlescale * stateScale : normalscale * stateScale;
    }

    bool interact = true;
    float stateScale = 1;
    public void NormalInteraction()
    {
        interact = true;
        stateScale = 1;
        myElementImage.color = Color.white;
        this.interactable = true;
        myElementImage.transform.localScale = normalscale * stateScale;
    }

    public void EnableInteraction(bool val)
    {
        if (val)
        {
            interact = true;
            stateScale = 1.05f;
            myElementImage.transform.localScale = littlescale * stateScale;
            myElementImage.color = Color.white;
            this.interactable = true;

            //this.interactable = true;
        }
        else
        {
            interact = false;
            stateScale = 1;
            myElementImage.transform.localScale = normalscale * stateScale;
            myElementImage.color = Color.black;
            this.interactable = false;
            //this.interactable = false;
        }
    }

    public void Refresh()
    {
        if (slot == null) return;
        if (slot.Stack == null)
        {
            Hide();
            return;
        }
        var stack = slot.Stack;
        if (!slot.Stack.IsEmptyOrElementNull)
        {
            var element = slot.Stack.Element;

            Show();
            myElementImage.sprite = element.Element_Image;
            myCant.enabled = true;
            myCant.text = stack.Quantity.ToString();

            isDirtyGameObject.SetActive(stack.IsDirty);
            isDryGameObject.SetActive(!stack.IsDry);
            //elementQuality.sprite = UI_InventoryDataBase.GetElementByQuality(stack.Quality);
            Tab_Object.SetActive(stack.IsTabbed);
            myElementImage.transform.localScale = stack.IsTabbed ? littlescale : normalscale;

            //if (element != null)
            //{

            //}
            //else
            //{
            //    Hide();
            //}
        }
        else
        {
            Hide();
        }
    }

    void Hide()
    {
        content.alpha = 0;
    }
    void Show()
    {
        content.alpha = 1;
        content.blocksRaycasts = false;
    }



    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop: " + gameObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        //if (Slot.IsEmpty) { Debug.LogWarning("el slot que Suelto, es null"); return; }

        if (eventData.pointerId == -1)
        {
            if (Input.GetKey(KeyCode.LeftControl)) SlotAction(inventory_input_action.ctrl_left_click, this);
            else SlotAction(inventory_input_action.left_up, this);
        }

        if (eventData.pointerId == -2)
        {
            SlotAction(inventory_input_action.right_up, this);
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (!interact) return;

        if (eventData.pointerId == -1)
        {
            SlotAction(inventory_input_action.left_down, this);
        }

        if (eventData.pointerId == -2)
        {
            SlotAction(inventory_input_action.right_down, this);
        }
    }



    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        myBkgSlot.sprite = UI_InventoryDataBase.SlotEnter;
        ElementInfo.Show(slot.Stack);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        myBkgSlot.sprite = UI_InventoryDataBase.SlotNormal;
        ElementInfo.Hide();
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        myBkgSlot.sprite = UI_InventoryDataBase.SlotEnter;
        ElementInfo.Show(slot.Stack);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        myBkgSlot.sprite = UI_InventoryDataBase.SlotNormal;
        ElementInfo.Hide();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!interact) return;
        if (slot.IsEmpty) return;
        DragAndDropCanvas.Drag(eventData.delta);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!interact) return;
        if (slot.IsEmpty) return;
        DragAndDropCanvas.BeginDrag(Element.Element_Image, myRect.anchoredPosition);
        if (eventData.pointerId == -1)
        {
            var enter = eventData.pointerEnter.GetComponent<UI_Slot>();
            if (enter)
            {
                InventoriesGestionController.instance.isDragging = true;
                enter.SlotAction(inventory_input_action.drag_begin, enter);
            }
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!interact) return;
        DragAndDropCanvas.EndDrag();
        if (eventData.pointerId == -1)
        {
            if (!eventData.pointerEnter)
            {
                InventoriesGestionController.instance.DropFail();
                return;
            }
            else
            {
                var enter = eventData.pointerEnter.GetComponent<UI_Slot>();
                if (enter)
                {
                    enter.SlotAction(inventory_input_action.drag_end, enter);
                    InventoriesGestionController.instance.isDragging = false;
                }
                else
                {
                    InventoriesGestionController.instance.DropFail();
                }
            }
        }
    }
}
