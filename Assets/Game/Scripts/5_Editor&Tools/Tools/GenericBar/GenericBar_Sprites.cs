using UnityEngine;
using UnityEngine.UI;
public class GenericBar_Sprites : GenericBar
{
    public Image sp = null;
    Color originalColor = Color.black;
    public Color  OriginalColor { get { return originalColor; } }
    [Header("opciones de gradiente")]
    [SerializeField] bool useGradient = false;
    [SerializeField] Gradient gradient = null;

    private void Start()
    {
        originalColor = sp.color;
    }
    protected override void OnSetValue(float value)
    {
        sp.fillAmount = value;
        if (useGradient)
        {
            sp.color = gradient.Evaluate(value);
        }
    }

    public void SetImageOriginalColor() { sp.color = originalColor; }
    public void SetImageColor(Color val) { sp.color = val; }

}
