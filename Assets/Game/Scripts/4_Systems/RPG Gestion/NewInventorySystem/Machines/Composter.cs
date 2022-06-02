using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composter : ConditionalElementComponent
{
    

    protected override bool CheckElement(StackedPile elem)
    {
        return 
            !elem.IsDry && 
                (elem.Element.ElementType == elementType.organic 
                || elem.Element.ElementType == elementType.paper);
    }

    public override void PlayCustomSoundOnClose()
    {
        
    }

    public override void PlayCustomSoundOnOpen()
    {
        SoundFX.Play_Machine_Open_Composter();
    }
}
