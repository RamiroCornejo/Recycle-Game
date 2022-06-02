using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftData", menuName = "ScriptableObjects/CraftData", order = 1)]
public class CraftData : ScriptableObject
{
    [Header("INPUT")]
    public ElementData input;

    [Header("Dry condition")]
    public bool IsImportant_DryState = false;
    public bool condition_value_isDry = true;

    [Header("Dirty condition")]
    public bool IsImportant_DirtyState = false;
    public bool condition_value_isDirty = false;

    [Header("OUTPUT")]
    public List<ElementData> output;
}
