using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefaultInteractable : GenericInteractable
{
    public UnityEvent OnEnter_Interact;
    public UnityEvent OnExit_Interact;
    public UnityEvent OnExecute_Interact;
    public UnityEvent OnBegin_Select;
    public UnityEvent OnExit_Select;

    protected override void OnBeginSelection()
    {
        OnBegin_Select.Invoke();   
    }

    protected override void OnEndSelection()
    {
        OnExit_Select.Invoke();
    }

    protected override void OnBeginOverlapInteract()
    {
        OnEnter_Interact.Invoke();
    }

    protected override void OnEndOverlapInteract()
    {
        OnExit_Interact.Invoke();
    }

    protected override void OnExecute()
    {
        OnExecute_Interact.Invoke();
    }
}
