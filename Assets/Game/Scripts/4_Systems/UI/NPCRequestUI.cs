using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRequestUI : MonoBehaviour
{
    [SerializeField] CanvasGroup group = null;

    [SerializeField] ItemRequestUI itemPrefab = null;
    [SerializeField] Transform parent = null;


    public void Initialize(ObjectData[] requireObjects)
    {
        for (int i = 0; i < requireObjects.Length; i++)
        {
            var item = Instantiate(itemPrefab, parent);
            item.SetItem(requireObjects[i].element.Element_Image, requireObjects[i].ammount, requireObjects[i].isDry, requireObjects[i].isDirty);
        }
    }

    public void Show()
    {
        group.alpha = 1;
    }

    public void Close()
    {
        group.alpha = 0;
    }
}
