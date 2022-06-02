using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum elementType { plastic, metal, organic, glass, paper, processed };

[CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/Element", order = 1)]
public class ElementData : ScriptableObject
{
    #region Editor variables
    [Header("Item Data")]
    [SerializeField] new string name = "default_element";
    [SerializeField] string pluralName = "elements";
    [SerializeField] int element_ID = -1;
    [SerializeField] int max_stack = 10;
    [SerializeField] [Multiline(4)] string description = "this is a element description";
    [SerializeField] float weight = 1.0f;
    [SerializeField] int max_quality;
    [SerializeField] float price;
    [SerializeField] elementType element_type;
    [SerializeField] bool recycled = false;

    [Header("Representation")]
    [SerializeField] Sprite image;
    [SerializeField] GameObject world_model;
    [SerializeField] GameObject equipable_model;
    #endregion

    #region Getters
    public string Element_Name => this.name;
    public string Plural_Name => pluralName;
    public int Element_ID => element_ID;
    public int MaxStack => max_stack;
    public string Description => description;
    public float Weight => weight;
    public Sprite Element_Image => image;
    public int MaxQuality => max_quality;
    public GameObject Model => world_model;
    public GameObject Model_Visual_Equipable => equipable_model;
    public float Price => price;
    public elementType ElementType => element_type;
    public bool Recycled => recycled;
    #endregion

}
