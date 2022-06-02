using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionalElementComponent : MonoBehaviour
{
    public bool Check(StackedPile elem) { return CheckElement(elem); }
    protected abstract bool CheckElement(StackedPile elem);

    public abstract void PlayCustomSoundOnOpen();
    public abstract void PlayCustomSoundOnClose();

}
