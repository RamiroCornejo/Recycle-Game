using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_by_anim_curve : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform elem1;
    [SerializeField] Transform elem2;

    private void Start()
    {
        Graphics_HIDE();
    }

    public void Graphics_SHOW()
    {
        elem1.gameObject.SetActive(true);
        elem2.gameObject.SetActive(true);
    }
    public void Graphics_HIDE()
    {
        elem1.gameObject.SetActive(false);
        elem2.gameObject.SetActive(false);
    }

    public void Lerp(float lerp_value) // 0 - 1
    {
        float rotation = curve.Evaluate(lerp_value);

        elem1.localEulerAngles = new Vector3(0, rotation, 0);
        elem2.localEulerAngles = new Vector3(0, rotation*-1, 0);
    }
}
