using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityModifier : MonoBehaviour
{
    [Range(0,100)]
    public int probability_to_dry = 50;
    [Range(0,100)]
    public int probability_to_dirty = 50;

    AddItemsToInventory adder;

    private void Awake()
    {
        adder = GetComponentInChildren<AddItemsToInventory>();
    }

    private void Start()
    {
        int dryvalue = Random.Range(0, 100);
        int dirtyvalue = Random.Range(0, 100);
        adder.isDry = dryvalue <= probability_to_dry;
        adder.isDirty = dirtyvalue <= probability_to_dirty;
    }
}
