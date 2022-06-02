using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreassureHittable : ShovelHittable
{
    [SerializeField] KeyValue<ElementData, float>[] elementsToSpawn = new KeyValue<ElementData, float>[0];

    ItemRecolectable SpawnObject(Vector3 position)
    {
        ItemRecolectable newItem = null;
        float spawnTotalProb = 0;
        for (int i = 0; i < elementsToSpawn.Length; i++)
        {
            spawnTotalProb += elementsToSpawn[i].value;
        }

        var randomValue = Random.Range(0.1f, spawnTotalProb);

        for (int i = 0; i < elementsToSpawn.Length; i++)
        {
            randomValue -= elementsToSpawn[i].value;

            if (randomValue <= 0)
            {
                newItem = GameManager.instance.GetItem(position);
                newItem.SetData(elementsToSpawn[i].key, true, IsDry, IsDirty);
                break;
            }
        }

        return newItem;
    }

    protected override void OnDead()
    {
        Vector3 spawnPos = transform.position;

        var obj = SpawnObject(spawnPos);
    }
}
