using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ElementInfo : MonoBehaviour
{
    public static ElementInfo instance;
    private void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI title;
    public Image img;
    public TextMeshProUGUI description;
    public TextMeshProUGUI weight;

    public GameObject state_wet;
    public GameObject state_dirty;
    public GameObject state_ready;

    CanvasGroup group;
    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.blocksRaycasts = false;
    }

    public static void Show(StackedPile stack) => instance.ShowInfo(stack);
    public static void Hide() => instance.HideInfo();

    void ShowInfo(StackedPile stack)
    {
        if (stack.IsEmptyOrElementNull) return;
        group.alpha = 1;
        title.text = stack.Element.Element_Name;
        img.sprite = stack.Element.Element_Image;
        description.text = stack.Element.Description;
        //weight.text = stack.Element.Weight.ToString();

        if (!stack.IsDirty && stack.IsDry)
        {
            state_wet.gameObject.SetActive(false);
            state_dirty.gameObject.SetActive(false);
            state_ready.gameObject.SetActive(true);
        }
        else
        {
            state_ready.gameObject.SetActive(false);
            state_dirty.gameObject.SetActive(stack.IsDirty);
            state_wet.gameObject.SetActive(!stack.IsDry);
        }

    }

    void HideInfo()
    {
        group.alpha = 0;
    }
}

