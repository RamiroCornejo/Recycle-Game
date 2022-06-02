using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentFilter : MonoBehaviour
{
    public bool Check(StackedPile obj)
    {
        return CheckFilter(obj);
    }

    public virtual bool CheckByElement(ElementData elem)
    {
        return false;
    }

    protected abstract bool CheckFilter(StackedPile obj);
}
