using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrashHittable : HittableBase
{
    [SerializeField] public ElementData elementToRepresent = null;
    [Range(0, 100)]
    public int probability_to_dry = 50;
    [Range(0, 100)]
    public int probability_to_dirty = 50;

    protected override void OnDead()
    {
        var newItem = GameManager.instance.GetItem(transform.position);
        newItem.SetData(elementToRepresent, true, IsDry, IsDirty);
    }

    protected bool IsDry => probability_to_dry > Random.Range(0, 100);
    protected bool IsDirty => probability_to_dirty > Random.Range(0, 100);
}
