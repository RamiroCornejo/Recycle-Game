using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaces : MonoBehaviour
{
    [SerializeField] List<Transform> positionsToSpawn = new List<Transform>();


    public Transform GetValidPlace()
    {
        if (positionsToSpawn.Count <= 0) return null;

        int random = Random.Range(0, positionsToSpawn.Count);
        var position = positionsToSpawn[random];
        positionsToSpawn.Remove(position);

        return position;
    }

    public void ReturnPlace(Transform position)
    {
        if (!positionsToSpawn.Contains(position))
            positionsToSpawn.Add(position);
    }

    public void DoNewList()
    {
        for (int i = 0; i < positionsToSpawn.Count; i++)
        {
            DestroyImmediate(positionsToSpawn[i].gameObject);
        }

        positionsToSpawn.Clear();

        positionsToSpawn = new List<Transform>(GetComponentsInChildren<Transform>());
        if (positionsToSpawn.Contains(transform)) positionsToSpawn.Remove(transform);
    }

    public void RemoveNulls()
    {
        positionsToSpawn = new List<Transform>(GetComponentsInChildren<Transform>());
        if (positionsToSpawn.Contains(transform)) positionsToSpawn.Remove(transform);
    }
}
