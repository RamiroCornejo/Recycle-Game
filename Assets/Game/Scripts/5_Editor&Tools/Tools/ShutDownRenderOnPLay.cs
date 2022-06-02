using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDownRenderOnPLay : MonoBehaviour
{
    private void Start()
    {
        var r = GetComponent<Renderer>();
        if(r) r.enabled = false;

        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;
    }
}
