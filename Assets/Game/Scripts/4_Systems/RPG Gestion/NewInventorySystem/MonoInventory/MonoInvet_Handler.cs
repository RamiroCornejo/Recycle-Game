using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInvet_Handler : MonoInvent_Base
{
    protected bool _isSelected;
    public bool IsSelected { get => _isSelected; }

    public ContentFilter content_filter;

    protected virtual void Start()
    {
        content_filter = GetComponent<ContentFilter>();
    }


    public bool Check_filter(StackedPile stacked)
    {
        if (content_filter != null)
        {
            return content_filter.Check(stacked);
        }
        else
        {
            return true;
        }
    }

    protected override void OnBuild() { }
    protected override void OnElementAdded(CPResult result) { }
    protected override void OnElementRemoved(CPResult result) { }
    protected override void OnMessage(string msg) { }
    protected override void OnOpenInventory() { _isSelected = true; }
    protected override void OnCloseInventory() { _isSelected = false; }
    protected override void OnRefresh(bool redraw = true) { }
}
