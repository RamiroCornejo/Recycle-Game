using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRequestUI : MonoBehaviour
{
    [SerializeField] Image img = null;
    [SerializeField] TextMeshProUGUI cantText = null;
    [SerializeField] GameObject isWetObj = null;
    [SerializeField] GameObject isDirtyObj = null;

    public void SetItem(Sprite sprt, int cant, bool isDry, bool isDirty)
    {
        img.sprite = sprt;
        cantText.text = cant.ToString();
        if (cant <= 1) cantText.gameObject.SetActive(false);
        isWetObj.SetActive(!isDry);
        isDirtyObj.SetActive(isDirty);
    }
}
