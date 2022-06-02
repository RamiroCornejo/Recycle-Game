using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;
using UnityEngine.Events;

public class AddItemsToInventory : MonoBehaviour
{
    [SerializeField] KeyValue<ElementData, int>[] items = new KeyValue<ElementData, int>[0];

    [SerializeField] Transform myTransform;

    [SerializeField] float anim_speed = 5;

    [SerializeField] UnityEvent OnTakeSucessfull;

    [SerializeField] UnityEvent OnEndAnimation;

    public bool isDry;
    public bool isDirty;

    float timer;
    bool anim;
    Vector3 anim_initial_pos;

    public bool hasOneShot = true;
    bool oneShot;

    private void Start()
    {

    }

    public void AddUniqueObject(ElementData data, int cant)
    {
        items = new KeyValue<ElementData, int>[1];
        items[0] = new KeyValue<ElementData, int>();
        items[0].key = data;
        items[0].value = cant;
    }

    public void AddItems()
    {
        if (hasOneShot)
        {
            if (oneShot) return;
            oneShot = true;
        }

        for (int i = 0; i < items.Length; i++)
        {
            var process_result = PlayerInventory.Add(items[i].key, items[i].value, 1, isDry, isDirty);

            if (process_result.Process_Successfull)
            {
                OnTakeSucessfull.Invoke();
                SoundFX.Play_Item_PickUp();
            }
            else
            {
                var quantity_to_drop = process_result.Remainder_Quantity;
                oneShot = false;
                return;
            }
        }

        anim = true;
        anim_initial_pos = myTransform.position;
    }

    private void Update()
    {
        #region ANIMATION
        if (anim)
        {
            if (timer < 1)
            {
                timer = timer + anim_speed * Time.deltaTime;
                myTransform.position = Vector3.Lerp(anim_initial_pos, Character.instance.transform.position, timer);
            }
            else
            {
                anim = false;
                timer = 0;
                OnEndAnimation.Invoke();
            }
        }
        #endregion
    }
}
