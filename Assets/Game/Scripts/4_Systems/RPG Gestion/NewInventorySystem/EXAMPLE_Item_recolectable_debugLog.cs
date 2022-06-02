using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXAMPLE_Item_recolectable_debugLog : MonoBehaviour
{
    public void Execute() => Debug.Log("estoy interactuando con..." + this.gameObject.name);
}
