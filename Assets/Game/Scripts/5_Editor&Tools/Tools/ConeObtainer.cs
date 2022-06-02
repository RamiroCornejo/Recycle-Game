using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeObtainer<T> where T : Component
{
    public static string Static_ToString() => "ObserverQuery";

    // public SpatialGrid targetGrid;
    float radius = 5f;
    float min_radius = 0f;
    float angle = 60;

    Transform target;
    Vector3 v_scaledetection;
    LayerMask layer;

    public void Configure(Transform target, LayerMask layer, float scale_detection = 2)
    {
        this.target = target;
        this.layer = layer;
        v_scaledetection = new Vector3(scale_detection, scale_detection, scale_detection);
    }

    public HashSet<T> Query()
    {
        HashSet<T> aux = new HashSet<T>();

        Collider[] elements = Physics.OverlapBox(target.transform.position, v_scaledetection, target.transform.rotation, layer);
        for (int i = 0; i < elements.Length; i++)
        {
            var elem = elements[i].GetComponent<T>();

            if (elem == null) continue;

            if (InRadius(elem.transform.position) && InAngle(elem.transform.position))
            {
                aux.Add(elem);
            }
        }

        return aux;
    }

    float dist;
    bool InRadius(Vector3 v3)
    {
        dist = (target.position - v3).sqrMagnitude;
        return dist < radius * radius && dist > min_radius * min_radius;
    }
    bool InAngle(Vector3 v3)
    {
        return Vector3.Angle(v3 - target.position, target.forward) < angle;
    }

    public void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, radius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(target.transform.position, min_radius);
        Gizmos.color = Color.yellow;
        var vdir = Quaternion.AngleAxis(angle, target.up) * target.forward;
        var vdir2 = Quaternion.AngleAxis(-angle, target.up) * target.forward;
        Gizmos.DrawRay(target.transform.position, vdir * radius);
        Gizmos.DrawRay(target.transform.position, vdir2 * radius);
    }
}
