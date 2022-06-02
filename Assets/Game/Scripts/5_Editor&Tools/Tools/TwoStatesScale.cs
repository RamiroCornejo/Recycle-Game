using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoStatesScale : MonoBehaviour
{
    [SerializeField] bool use_own;
    [SerializeField] Transform myTransform;
    [SerializeField] Vector3 scaleA;
    [SerializeField] Vector3 scaleB;

    private void Awake()
    {
        if (use_own) myTransform = this.transform;
    }

    public void Execute_ScaleA()
    {
        myTransform.localScale = scaleA;
    }

    public void Execute_ScaleB()
    {
        myTransform.localScale = scaleB;
    }
}
