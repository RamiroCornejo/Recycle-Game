using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class HitSensor : MonoBehaviour
{
    [SerializeField] float offset = 1.5f;
    [SerializeField] LayerMask mask = 0 << 0;
    List<HittableBase> hittables = new List<HittableBase>();
    HittableBase currentNearest;

    bool hiting;

    public void UpdateDirection(float x, float y, bool sprite, Vector3 parentPos)
    {
        if (!hiting) return;
        RefreshPosition(x, y, sprite, parentPos);
        hittables = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity, mask)
            .Where(x => x.GetComponent<HittableBase>()).Select(x => x.GetComponent<HittableBase>()).ToList();
        if (!hittables.Contains(currentNearest))
        {
            currentNearest?.OnResetHittable();
            ReleaseHit(x, y, sprite, parentPos);
        }
    }


    public bool ExecuteHit(float x, float y, bool sprite, Vector3 parentPos)
    {
        if (hiting) return false;

        RefreshPosition(x, y, sprite, parentPos);
        var nearest = GetNearest();
        if (!nearest) return false;

        if (Character.instance.CheckTools(nearest.RequiredTool()))
        {
            nearest.Hit(RemoveHittable);
            currentNearest = nearest;
            hiting = true;
            return true;
        }
        else
        {
            nearest.ExecuteErrorMesseage();
            return false;
        }
    }

    public bool ReleaseHit(float x, float y, bool sprite, Vector3 parentPos)
    {
        if (!hiting) return false;
        hiting = false;
        currentNearest?.ReleaseHit();
        RefreshPosition(x, y, sprite, parentPos);
        var nearest = GetNearest();
        if (!nearest) return false;

        return true;
    }

    void RefreshPosition(float x, float y, bool sprite, Vector3 parentPos)
    {
        if (x != 0) y = 0;

        var clampedX = Mathf.Clamp(x + 10000 * x, -1, 1);
        var clampedY = Mathf.Clamp(y + 10000 * y, -1, 1);

        if(clampedX == 0 && clampedY == 0)
        {
            if (!sprite) transform.position = new Vector3(parentPos.x + offset, parentPos.y, parentPos.z);
            else transform.position = new Vector3(parentPos.x - offset, parentPos.y, parentPos.z);
        }
        else
        {
            transform.position = new Vector3(parentPos.x + offset * clampedX, parentPos.y, parentPos.z + offset * clampedY);
        }
    }

    HittableBase GetNearest()
    {
        hittables = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity, mask)
                    .Where(x => x.GetComponent<HittableBase>()).Select(x => x.GetComponent<HittableBase>()).ToList();
        if (hittables.Count <= 0) return null;

        HittableBase nearest = hittables[0];
        Vector3 charPos = Character.instance.transform.position;
        float distance = Vector3.Distance(charPos, nearest.transform.position);

        for (int i = 1; i < hittables.Count; i++)
        {
            float distanceTwo = Vector3.Distance(charPos, hittables[i].transform.position);

            if(distance > distanceTwo)
            {
                distance = distanceTwo;
                nearest = hittables[i];
            }
        }

        return nearest;
    }

    public void RemoveHittable(HittableBase hittable)
    {
        if (hittables.Contains(hittable)) hittables.Remove(hittable);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    var hittable = other.GetComponent<HittableBase>();
    //    if (!hittable) return;

    //    if (!hittables.Contains(hittable)) hittables.Add(hittable);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    var hittable = other.GetComponent<HittableBase>();
    //    if (!hittable) return;

    //    if (hittables.Contains(hittable)) hittables.Remove(hittable);
    //}
}
