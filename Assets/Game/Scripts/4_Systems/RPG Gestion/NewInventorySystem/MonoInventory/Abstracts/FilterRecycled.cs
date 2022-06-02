using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum elementTypeCustom { secadora, compostadora, lavadora, horno_vidrio, proc_plastico, prensa, container }
public class FilterRecycled : ContentFilter
{
    [SerializeField] elementType elemtype;
    public bool isCustom;
    public elementTypeCustom custom_elem;
    MonoInvent_Base container;
    [SerializeField] int ContainerType_MaxQuantity;
    private void Start()
    {
        if (custom_elem == elementTypeCustom.container) {
            container = GetComponent<MonoInvent_Base>();
            if (container == null) throw new System.Exception("Esta Marcado como Container, pero no tengo uno");
        }
    }
    protected override bool CheckFilter(StackedPile obj)
    {
        if (obj.Element == null) throw new System.Exception("El elemento que queremos chekear es null");

        if (!isCustom)
        {
            return
            obj.IsDry &&
           !obj.IsDirty &&
           obj.Element.ElementType == elemtype;
        }
        else
        {
            switch (custom_elem)
            {
                case elementTypeCustom.secadora: return !obj.IsDry;
                case elementTypeCustom.compostadora: return !obj.IsDry && obj.Element.ElementType == elementType.organic;
                case elementTypeCustom.lavadora: return true;
                case elementTypeCustom.horno_vidrio: return obj.IsDry && !obj.IsDirty && obj.Element.ElementType == elementType.glass;
                case elementTypeCustom.proc_plastico: return obj.IsDry && !obj.IsDirty && obj.Element.ElementType == elementType.plastic;
                case elementTypeCustom.prensa: return obj.IsDry && !obj.IsDirty && obj.Element.ElementType == elementType.metal;
                case elementTypeCustom.container: return true;
            }
            return true;
        }
    }

    public override bool CheckByElement(ElementData elem)
    {

        int sum = 0;
        for (int i = 0; i < container.Container.Capacity; i++)
        {
            var slot = container.GetSlotByIndex(i);
            if (!slot.IsEmpty)
            {
                if (slot.Element.Equals(elem))
                {
                    sum += slot.Stack.Quantity;
                }
            }
        }

        return (sum <= ContainerType_MaxQuantity);
    }
}
