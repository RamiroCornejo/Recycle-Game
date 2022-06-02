using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class GenericInteractor : MonoBehaviour
{

    public TextMeshProUGUI interactables_debug;

    HashSet<GenericInteractable> interactables = new HashSet<GenericInteractable>();
   GenericInteractable most_close;

    Action<GenericInteractable> OnAddInteractable = delegate { };
    Action<GenericInteractable> OnRemoveInteractable = delegate { };

    Func<GenericInteractable, bool> Predicate = delegate { return true; };

    //public List<GenericInteractable> debugger = new List<GenericInteractable>();

    bool isStoped;

    //private void Update()
    //{
    //    debugger = new List<GenericInteractable>();
    //    foreach (var i in interactables)
    //    {
    //        debugger.Add(i);
    //    }
    //}

    void RefreshDebug()
    {
        if (!interactables_debug) return;
        interactables_debug.text = "[INTERACTABLES]\n";
        foreach (var i in interactables)
        {
            
            interactables_debug.text += (i == most_close ? "<color=#E0E300>" : "") + "[" + i.gameObject.name + (i == most_close ? "</color>" : "") +"]\n"; 
        }
    }

    public void AddInteractable(GenericInteractable i)
    {
        interactables.Add(i);
        OnAddInteractable.Invoke(i);
        RefreshDebug();
    }
    public void RemoveInteractable(GenericInteractable i)
    {
        interactables.Remove(i);
        OnRemoveInteractable.Invoke(i);
        RefreshDebug();
    }
    public void ADD_CALLBACK_Add_Interactable(Action<GenericInteractable> callback_add_interactable) => OnAddInteractable = callback_add_interactable;
    public void ADD_CALLBACK_Remove_Interactable(Action<GenericInteractable> callback_remove_interactable) => OnRemoveInteractable = callback_remove_interactable;

    private void OnDestroy()
    {
        StopCoroutine(Refresh());
    }

    Dictionary<string, Tuple<HashSet<GenericInteractable>, Func<GenericInteractable, bool>>> filters = new Dictionary<string, Tuple<HashSet<GenericInteractable>, Func<GenericInteractable, bool>>>();

    public IEnumerator GetInteractorsByPredicate(Func<GenericInteractable, bool> Pred)
    {
        float distance = float.MaxValue;
        foreach (var i in interactables)
        {
            var currentdistance = Vector3.Distance(this.transform.position, i.transform.position);

            if (currentdistance < distance && Pred(i))
            {
                distance = currentdistance;
                most_close = i;
            }
            yield return null;
        }
    }

    public void AddFilter(string filterName, Func<GenericInteractable, bool> predicate)
    {
        if (!filters.ContainsKey(filterName))
        {
            HashSet<GenericInteractable> col = new HashSet<GenericInteractable>();
            filters.Add(filterName, Tuple.Create(col, predicate));
        }
    }
    public HashSet<GenericInteractable> GetFilteredCollection(string filterName)
    {
        if (filters.ContainsKey(filterName))
        {
            return filters[filterName].Item1;
        }
        else return null;

    }

    public void InitializeInteractor()
    {
        StartCoroutine(Refresh());
    }
    public void DeinitializeInteractor()
    {
        StopCoroutine(Refresh());
    }

    public void ConfigurePredicate(Func<GenericInteractable, bool> predicate) => this.Predicate = predicate;
    public void RemovePredicate(Func<GenericInteractable, bool> predicate) => this.Predicate = delegate { return true; };

    float lastInterval;
    IEnumerator Refresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            foreach (var f in filters)
            {
                f.Value.Item1.Clear();
            }

            float distance = float.MaxValue;

            var old_most_close = most_close;
            most_close = null;

            foreach (var i in interactables)
            {
                var currentdistance = Vector3.Distance(this.transform.position, i.transform.position);

                foreach (var f in filters)
                {
                    if (f.Value.Item2(i)) //si cumple con algunos de los filtros lo agrego a la base de datos
                    {
                        f.Value.Item1.Add(i);
                    }
                }

                if (currentdistance < distance)
                {
                    distance = currentdistance;
                    most_close = i;
                }
                else
                {
                    // si no estaba en distancia y a la vez estaba seleccionado
                    // quiere decir que se movio y se alejó, lo deselecciono
                    if (i.IsSelected)
                    {
                        i.EndSelection();
                        SelectionFeedback.Hide();
                    }
                }
                //yield return null;
            }

            if (old_most_close != null)
            {
                if (old_most_close != most_close)
                {
                    //evento de Cambio de posicion
                    old_most_close.EndSelection();

                    if (most_close)
                    {
                        most_close.BeginSelection();
                        SelectionFeedback.SetPosition(most_close.InteractPosition);
                    }
                }
                else
                {
                    //no hago nada xq es el mismo
                }
            }
            else
            {
                if (most_close)
                {
                    most_close.BeginSelection();
                    //evento de nueva posicion
                    SelectionFeedback.SetPosition(most_close.InteractPosition, true);
                }
                else
                {
                    //si no tengo al anterior y a su vez tampoco al nuevo me salgo
                    //evento salgo
                    SelectionFeedback.Hide();
                }
            }

            old_most_close = null;
            if(most_close == null) SelectionFeedback.Hide();

            RefreshDebug();
        }
    }

    public void Execute()
    {
        if (most_close != null)
        {
            most_close.Execute();
        }
    }
}
