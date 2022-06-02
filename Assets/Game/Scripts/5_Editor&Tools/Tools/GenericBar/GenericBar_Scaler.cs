using UnityEngine;
public class GenericBar_Scaler : GenericBar
{
    [SerializeField] Transform pivotPointToScale = null;
    [SerializeField] axis _axis = axis.x;
    float scaler = 0f;
    public void ConfigureScaler(float _scaler) => scaler = _scaler;
    protected override void OnSetValue(float value)
    {
        Vector3 scale = new Vector3(1, 1, 1);
        if (_axis == axis.x) scale.x = percentage;
        if (_axis == axis.y) scale.y = percentage;
        if (_axis == axis.z) scale.z = percentage;
        pivotPointToScale.localScale = scale;
    }
    [System.Serializable]
    public enum axis
    {
        x,
        y,
        z
    }
}
