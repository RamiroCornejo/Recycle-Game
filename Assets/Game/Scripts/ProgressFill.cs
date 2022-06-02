using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressFill : MonoBehaviour
{
    public Image myFill;
    public CanvasGroup myGroup;

    public void Fill(float val)
    {
        myFill.fillAmount = val;
    }

    public void Open()
    {
        myGroup.alpha = 1;
    }
    public void Close()
    {
        myGroup.alpha = 0;
    }
}
