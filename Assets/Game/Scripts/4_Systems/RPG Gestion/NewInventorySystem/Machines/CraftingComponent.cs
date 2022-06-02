using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingComponent : MonoBehaviour
{
    [SerializeField] CraftData[] possibleCraftings;

    public bool QueryElement(StackedPile elem)
    {
        for (int i = 0; i < possibleCraftings.Length; i++)
        {
            var craft = possibleCraftings[i];

            if (craft.input.Equals(elem.Element))
            {
                bool condA = false;
                bool condB = false;

                if (craft.IsImportant_DryState)
                {
                    if (craft.condition_value_isDry == elem.IsDry) condA = true;//es importante y cumple
                    else continue; //es importante y no cumple
                }
                else condA = true;

                if (craft.IsImportant_DirtyState)
                {
                    if (craft.condition_value_isDirty == elem.IsDirty) condB = true; //es importante y cumple
                    else continue; //es importante y no cumple
                }
                else condB = true;

                if (condA && condB) return true;
                else continue;
            }
        }
        return false;
    }

    public List<ElementData> GetOutputByInput(StackedPile elem)
    {
        for (int i = 0; i < possibleCraftings.Length; i++)
        {
            var craft = possibleCraftings[i];

            if (craft.input.Equals(elem.Element))
            {
                return craft.output;
            }
        }
        return null;
    }
}
