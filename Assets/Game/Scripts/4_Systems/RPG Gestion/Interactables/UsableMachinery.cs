using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableMachinery : GenericInteractable
{
    DefaultMachinery myMachinery;
    [SerializeField] GameObject fast_feedback;
    protected override void Start()
    {
        base.Start();
        myMachinery = GetComponent<DefaultMachinery>();
    }

    /*
     * el SELECTION a diferencia del Overlap, este indica que
     * comenzó a ser "Target de Seleccion".
     * Si el interactor Ejecuta, me va a Ejecutar a mi por que
     * soy su Target.
     */
    protected override void OnBeginSelection() { myMachinery.EnterSelection(); if(fast_feedback) fast_feedback.gameObject.SetActive(true); }
    protected override void OnEndSelection() { myMachinery.ExitSelection(); if (fast_feedback) fast_feedback.gameObject.SetActive(false); }

    //si me tiene Overlap y al mismo tiempo soy su target
    //El Execute me manda acá
    protected override void OnExecute() => myMachinery.Execute();

    #region Overlap [En Desuso]
    /*
     * el OVERLAP es cuando el Interactor entra adentro de
     * el trigger, pero es independiente de si lo tengo 
     * como "target de seleccion"
     * si el interactor Ejecuta tal vez no me ejecute porque 
     * tal vez no sea su target
     */
    protected override void OnBeginOverlapInteract()
    {

    }
    protected override void OnEndOverlapInteract()
    {

    }
    #endregion
}
