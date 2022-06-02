using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prensa : ConditionalElementComponent
{
    

    protected override bool CheckElement(StackedPile elem)
    {
        return
            elem.IsDry &&
            !elem.IsDirty &&
                elem.Element.ElementType == elementType.metal;
    }


    public override void PlayCustomSoundOnClose()
    {
        
    }

    public override void PlayCustomSoundOnOpen()
    {
        SoundFX.Play_Machine_Open_Press();
    }
}
