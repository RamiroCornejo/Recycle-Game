using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] AddItemsToInventory itemToRepresent = null;
    [SerializeField] SpriteRenderer model = null;
    [SerializeField] float spit_force = 2f;
    [SerializeField] GameObject dryObj = null;
    [SerializeField] GameObject dirtyObj = null;
    [SerializeField] Animator anim = null;

    float timer;
    [SerializeField] float lifeTime = 30;
    bool hasLifeTime;

    private void Update()
    {
        if (hasLifeTime)
        {
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                timer = 0;
                hasLifeTime = false;
                ReturnToPool();
            }
        }
    }

    public void ResetItem()
    {
        rb.velocity = Vector3.zero;
        hasLifeTime = false;
        timer = 0;
    }

    public void SetData(ElementData data, bool _hasLifeTime, bool isDry = true, bool isDirty = false)
    {
        hasLifeTime = _hasLifeTime;
        itemToRepresent.AddUniqueObject(data, 1);
        itemToRepresent.isDry = isDry;
        itemToRepresent.isDirty = isDirty;
        itemToRepresent.hasOneShot = false;
        model.sprite = data.Element_Image;
        dryObj.SetActive(!isDry);
        dirtyObj.SetActive(isDirty);
        SetAnimatorSpeed(1);
    }
    public void SetDataDryAndClean(ElementData data, bool _hasLifeTime)
    {
        SetData(data, _hasLifeTime, true, false);
    }

    public void SpitOnDirection(Vector3 dir)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * spit_force, ForceMode.VelocityChange);
    }

    public void ReturnToPool()
    {
        GameManager.instance.ReturnItem(this);
    }

    public void SetAnimatorSpeed(int speed)
    {
        anim.enabled = speed == 1 ? true:false;
    }
}
