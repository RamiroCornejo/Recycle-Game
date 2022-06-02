using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleCollisionSplat : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] ParticleSystem splat;

    private void Awake()
    {
        ParticlesManager.Instance.GetParticlePool(splat.name, splat);
        particle = GetComponent<ParticleSystem>();
    }


    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> list = new List<ParticleCollisionEvent>();
        var particleCollisionCount = particle.GetCollisionEvents(other, list);
        for (int i = 0; i < particleCollisionCount; i++)
        {
            Debug.Log(list[i].intersection);
            ParticlesManager.Instance.PlayParticle(splat.name, list[i].intersection);
        }
    }
}
