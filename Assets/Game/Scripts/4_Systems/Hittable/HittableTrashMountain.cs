using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableTrashMountain : TrashHittable
{
    [SerializeField] int maxLife = 4;
    int currentLife = 0;
    [SerializeField] KeyValue<ElementData, float>[] elementsToSpawn = new KeyValue<ElementData, float>[0];

    [SerializeField] int minObjectsToSpawn = 4;
    [SerializeField] int maxObjectsToSpawn = 5;

    [SerializeField] Animator anim = null;
    

    private void Start()
    {
        currentLife = maxLife;
    }

    public override void OnResetHittable()
    {
        currentLife = maxLife;
    }

    protected override void OnExecuteErrorMesseage()
    {
    }

    protected override void OnHit()
    {
        currentLife -= 1;

        if(currentLife <= 0)
        {
            Dead();
        }
        else
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        anim.SetTrigger("Hit");
        SpawnObject(transform.position);
        SoundFX.Play_HitGarbage();
    }

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

            if(randomValue<= 0)
            {
                newItem = GameManager.instance.GetItem(position);
                newItem.SetData(elementsToSpawn[i].key, true, IsDry, IsDirty);
                break;
            }
        }

        return newItem;
    }

    protected override void OnReleaseHit()
    {
    }

    protected override void OnDead()
    {
        var random = Random.Range(minObjectsToSpawn, maxObjectsToSpawn);
        Vector3 dir = -(Character.instance.transform.position - transform.position).normalized;
        dir.y = 2;
        Vector3 spawnPos = transform.position + dir;

        for (int i = 0; i < random; i++)
        {
            var obj = SpawnObject(spawnPos);
            obj.GetComponent<Rigidbody>().AddForce(dir * Random.Range(1, 5), ForceMode.VelocityChange);
        }
    }
}
