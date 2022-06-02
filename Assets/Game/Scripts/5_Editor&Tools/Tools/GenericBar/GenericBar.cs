using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class GenericBar : MonoBehaviour
{
    protected float maxValue = 0f;
    [Header("para visualizar la barra")]
    [SerializeField] protected Text percent_legacy = null;
    [SerializeField] protected TextMeshProUGUI percent_TMPro = null;
    [SerializeField] protected bool userealvalue = false;
    protected int percentage = 0;
    float realvalue = 0f;
    float lerpvalue = 0f;

    float EnterValue
    {
        set
        {
            realvalue = value; //el valor crudo
            lerpvalue = value / maxValue; //el valor para lerpeo 0.25f
            percentage = (int)(lerpvalue * 100); //el valor de porcentaje para mostrar 75%
        }
    }

    public string MyText => !userealvalue ? percentage.ToString() + "%" : realvalue + " / " + maxValue;

    public void Configure(float maxValue, float initial_value = 0f)
    {
        this.maxValue = maxValue;
        EnterValue = initial_value;
        RefreshTexts();
    }

    void RefreshTexts()
    {
        if (percent_legacy) percent_legacy.text = MyText;
        if (percent_TMPro) percent_TMPro.text = MyText;
    }

    public void SetValue(float val)
    {
        EnterValue = val;
        OnSetValue(lerpvalue);
        RefreshTexts();
    }
    protected abstract void OnSetValue(float val);
    
}
