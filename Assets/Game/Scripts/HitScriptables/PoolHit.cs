using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolHit : SingleObjectPool<HittableBase>
{

    [SerializeField] private HittableBase particle;
    public bool soundPoolPlaying = false;
    public float duration;

    public void Configure(HittableBase _particle, float _duration)
    {
        extendible = true;
        particle = _particle;
        duration = _duration;
    }

    protected override void AddObject(int prewarm = 3)
    {
        var newParticle = Instantiate(particle);
        newParticle.gameObject.SetActive(false);
        newParticle.transform.SetParent(transform);
        objects.Enqueue(newParticle);
    }

    public void ReturnParticle(HittableBase particle)
    {
        if (particle == null) return;

        particle.OnResetHittable();
        particle.transform.SetParent(transform);
        ReturnToPool(particle);
    }
}
