using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PobreTool : MonoBehaviour
{
    [SerializeField] int gridX = 1;
    [SerializeField] int gridZ = 1;

    [SerializeField] float spaceX = 1;
    [SerializeField] float spaceZ = 1;

    [SerializeField] bool spawn = false;
    [SerializeField] bool removeNulls = false;
    [SerializeField] GameObject prefab = null;

    [SerializeField] SpawnPlaces places = null;

    private void Update()
    {
        if (spawn)
        {
            var current = transform.position;
            var initZ = current.z;
            for (int x = 0; x < gridX; x++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Instantiate(prefab, current, Quaternion.identity, places.transform);
                    current.z += spaceZ;
                }
                current.z = initZ;
                current.x += spaceX;
            }
            places.DoNewList();

            spawn = false;
        }

        if (removeNulls)
        {
            places.RemoveNulls();

            removeNulls = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + (gridX * spaceX / 2), transform.position.y, transform.position.z + (gridZ * spaceZ / 2)),
                            new Vector3(gridX * spaceX, 2, gridZ * spaceZ));
    }
}
