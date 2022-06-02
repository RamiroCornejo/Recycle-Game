using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnerDataBase dataBase = new SpawnerDataBase();
    [SerializeField] SpawnPlaces places = null;

    Dictionary<HittableBase, Func<int, int>> originalPercents = new Dictionary<HittableBase, Func<int, int>>();

    List<HittableBase> allHittables = new List<HittableBase>();

    bool spawning;

    private void Start()
    {
        int cant = dataBase.GetCant();
        for (int i = 0; i < cant; i++)
        {
            var element = dataBase.GetElement(i);
            HittablePool.Instance.GetParticlePool(element.name, element);
        }

        Spawn();
    }

    public void ResetZone()
    {
        if (spawning) return;
        for (int i = 0; i < allHittables.Count; i++)
        {
            allHittables[i].ReturnObject();
        }

        allHittables.Clear();

        Spawn();
    }

    void Spawn()
    {
        if (spawning) return;
        spawning = true;

        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var objectsToSpawn = dataBase.GetCant();

        for (int i = 0; i < objectsToSpawn; i++)
        {
            var element = dataBase.GetTotalElement(i);

            int ammountToSpawn = Random.Range(dataBase.GetMinSpawn(i), dataBase.GetMaxSpawn(i));

            if (originalPercents.ContainsKey(element.key))
            {
                ammountToSpawn = originalPercents[element.key].Invoke(ammountToSpawn);
            }

            for (int a = 0; a < ammountToSpawn; a++)
            {
                var newObject = HittablePool.Instance.PlayParticle(element.key.name, Vector3.zero);

                var posToSpawn = places.GetValidPlace();

                newObject.Spawn(posToSpawn, places.ReturnPlace, element.key.name, this);
                allHittables.Add(newObject);

                if (stopwatch.ElapsedMilliseconds < 1f / 30f)
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
        }
        spawning = false;
    }

    public void ReturnHittable(string poolName, HittableBase hit)
    {
        HittablePool.Instance.ReturnToPool(poolName, hit);
        if(allHittables.Contains(hit))allHittables.Remove(hit);
    }

    public void DecreaseAppearProb(HittableBase element, Func<int, int> stackOnChest)
    {
        originalPercents[element] = stackOnChest;
    }
}
