using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericInteractable : MonoBehaviour
{
    HashSet<GenericInteractor> interactors = new HashSet<GenericInteractor>();
    GenericInteractor lastInteractor;
    public enum exclude { nothing, player }
    public exclude excludes;
    bool isSelected;
    public bool IsSelected => isSelected;

    // [quitOnExecute] esta variable si esta en true hace que cuando ejecutas este
    // interactable se remueve de la lista de interuables del interactor. Esto hace que
    // tengas que salir del trigger y volver a entrar para entrar a la seleccion
    // [false] para maquinas y objetos estaticos que no se van a destruir o mover 
    // [true] para recolectables que necesitamos un OneShot
    [SerializeField] bool quitOnExecute = false;

    [SerializeField] bool oneShot = false;

    [SerializeField] Transform interact_point;
    public Vector3 InteractPosition
    {
        get
        {
            if (interact_point == null) return this.transform.position + Vector3.up * 3;
            else return interact_point.position;
        }
    }

    bool alreadyExecute;
    public bool IsQuitOnExecute => quitOnExecute;

    protected virtual void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (alreadyExecute) return;
        var interactor = other.GetComponent<GenericInteractor>();

        if (interactor != null)
        {
            AddToInteractor(interactor);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var interactor = other.GetComponent<GenericInteractor>();

        if (interactor != null)
        {
            if (excludes == exclude.player)
            {
                var player = interactor.GetComponent<Character>();
                if (player != null) return;
            }

            OnEndOverlapInteract();
            interactors.Remove(interactor);
            interactor.RemoveInteractable(this);
            if (isSelected)
            {
                EndSelection();
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var i in interactors)
        {
            i.RemoveInteractable(this);
        }
        interactors.Clear();
    }
    private void OnDisable()
    {
        foreach (var i in interactors)
        {
            i.RemoveInteractable(this);
        }
        interactors.Clear();
    }

    public void AddToInteractor(GenericInteractor interactor)
    {
        if (excludes == exclude.player)
        {
            var player = interactor.GetComponent<Character>();
            if (player != null) return;
        }

        OnBeginOverlapInteract();
        interactors.Add(interactor);
        interactor.AddInteractable(this);
        lastInteractor = interactor;
    }

    public void AddToInteractor()
    {
        if (excludes == exclude.player)
        {
            var player = lastInteractor.GetComponent<Character>();
            if (player != null) return;
        }

        OnBeginOverlapInteract();
        interactors.Add(lastInteractor);
        lastInteractor.AddInteractable(this);
    }

    public void Execute()
    {
        OnExecute();

        if (!quitOnExecute) return;

        foreach (var i in interactors)
        {
            i.RemoveInteractable(this);
        }
        interactors.Clear();
        OnEndOverlapInteract();
        if (isSelected)
        {
            EndSelection();
        }
        if (oneShot) alreadyExecute = true;
    }

    public void BeginSelection()
    {
        isSelected = true;
        OnBeginSelection();
    }
    public void EndSelection()
    {
        isSelected = false;
        OnEndSelection();
    }

    protected abstract void OnBeginOverlapInteract();
    protected abstract void OnEndOverlapInteract();
    protected abstract void OnExecute();
    protected abstract void OnBeginSelection();
    protected abstract void OnEndSelection();
}
