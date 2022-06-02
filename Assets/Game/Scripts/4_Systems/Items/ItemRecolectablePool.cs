using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRecolectablePool
{
    PoolItem itemPool;

    public ItemRecolectablePool(ItemRecolectable prefab, Transform parent)
    {
        itemPool = new GameObject("itemsPool").AddComponent<PoolItem>();
        itemPool.transform.SetParent(parent);
        itemPool.Configure(prefab, 0);
        itemPool.Initialize(5);
    }

    public ItemRecolectable SpawnItem(Vector3 spawnPos)
    {
        ItemRecolectable aS = itemPool.Get();
        aS.transform.position = spawnPos;

        return aS;
    }

    public void ReturnItem(ItemRecolectable item)
    {
        itemPool.ReturnParticle(item);
    }
}
