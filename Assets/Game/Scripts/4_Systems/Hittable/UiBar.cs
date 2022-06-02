using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] GameObject generalHUD;
    [SerializeField] float barOffset = 2;
    Transform character;
    HittableWBar currentOwner;

    [SerializeField] float max = 10;
    float current;

    private void Start()
    {
        character = Character.instance.transform;
    }

    public bool IsEmpty { get => current <= 0; }
    public bool IsFilled { get => current >= max; }

    public bool AddToBar(float numToBar = 1)
    {
        current += numToBar;
        RefreshBar();
        if (current >= max)
        {
            current = max;
            return true;
        }
        return false;
    }

    public bool RestToBar(float numToBar = 1)
    {
        current -= numToBar;
        RefreshBar();
        if (current <= 0)
        {
            current = 0;
            return true;
        }
        return false;
    }


    private void LateUpdate()
    {
        if (IsEmpty) return;
        Vector3 charPosition = new Vector3(character.position.x, character.position.y + barOffset, character.position.z);

        generalHUD.transform.position = Camera.main.WorldToScreenPoint(charPosition);
    }

    void RefreshBar() => bar.fillAmount = current / max;

    public void ResetBar()
    {
        current = 0;
        RefreshBar();
    }

    public void ShutDown()
    {
        generalHUD.SetActive(false);
        ResetBar();
    }

    public void ShutOn(HittableWBar owner)
    {
        if (owner != currentOwner &&currentOwner != null) currentOwner.barAuth = false;
        currentOwner = owner;
        currentOwner.barAuth = true;
        generalHUD.SetActive(true);
        ResetBar();
    }
}
