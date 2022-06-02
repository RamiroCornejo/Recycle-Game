using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class SpawnerDataBase
{
    [SerializeField] KeyValue<HittableBase, KeyValue<int, int>>[] elementToSpawn = new KeyValue<HittableBase, KeyValue<int, int>>[0];


    public HittableBase GetElement(int i) => elementToSpawn[i].key;

    public int GetMaxSpawn(int i) => elementToSpawn[i].value.value;

    public int GetMinSpawn(int i) => elementToSpawn[i].value.key;

    public int GetCant() => elementToSpawn.Length;

    public KeyValue<HittableBase, KeyValue<int, int>> GetTotalElement(int i)
    {
        return elementToSpawn[i];
    }
}
