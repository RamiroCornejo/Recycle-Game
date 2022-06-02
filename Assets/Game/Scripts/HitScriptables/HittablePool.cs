using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittablePool : MonoBehaviour
{
    public static HittablePool Instance { get; private set; }

    private Dictionary<string, PoolHit> particleRegistry = new Dictionary<string, PoolHit>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public HittableBase PlayParticle(string particleName, Vector3 spawnPos, Transform trackingTransform = null)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            var particlePool = particleRegistry[particleName];
            HittableBase aS = particlePool.Get();
            aS.transform.position = spawnPos;
            if (trackingTransform != null) aS.transform.SetParent(trackingTransform);

            return aS;
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en el pool");
            return null;
        }
    }

    public void ReturnToPool(string particleName, HittableBase hittable)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            var particlePool = particleRegistry[particleName];
            particlePool.ReturnParticle(hittable);
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en el pool");
        }
    }

    public PoolHit GetParticlePool(string particleName, HittableBase particle = null, int prewarmAmount = 2)
    {
        if (particleRegistry.ContainsKey(particleName)) return particleRegistry[particleName];
        else if (particle != null) return CreateNewParticlePool(particle, particleName, prewarmAmount);
        else return null;
    }

    public void DeleteParticlePool(string particleName)
    {
        if (particleRegistry.ContainsKey(particleName))
        {
            Destroy(particleRegistry[particleName].gameObject);
            particleRegistry.Remove(particleName);
        }
    }

    #region Internal
    private PoolHit CreateNewParticlePool(HittableBase particle, string particleName, int prewarmAmount = 2)
    {
        var particlePool = new GameObject($"{particleName} soundPool").AddComponent<PoolHit>();
        particlePool.transform.SetParent(transform);
        particlePool.Configure(particle, 0);
        particlePool.Initialize(prewarmAmount);
        particleRegistry.Add(particleName, particlePool);
        return particlePool;
    }
    #endregion
}
