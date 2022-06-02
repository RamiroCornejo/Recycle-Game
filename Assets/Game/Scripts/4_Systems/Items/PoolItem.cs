using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolItem : SingleObjectPool<ItemRecolectable>
{

    [SerializeField] private ItemRecolectable particle;
    public bool soundPoolPlaying = false;
    public float duration;

    public void Configure(ItemRecolectable _particle, float _duration)
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

    public void ReturnParticle(ItemRecolectable particle)
    {
        if (particle == null) return;

        particle.ResetItem();
        particle.transform.SetParent(transform);
        ReturnToPool(particle);
    }
}
