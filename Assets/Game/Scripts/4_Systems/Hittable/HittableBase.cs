using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class HittableBase : MonoBehaviour
{
    [SerializeField] protected CharacterTools requiredTool = CharacterTools.Pikes;

    [SerializeField] Message messComp = null;
    [SerializeField] string errorText = "No le podés pegar pá";

    [SerializeField] ParticleSystem deadPS;

    Transform myPlace;
    Action<Transform> ReturnSpawn;
    protected string poolName;
    public CharacterTools RequiredTool() => requiredTool;

    Action<HittableBase> removeMe;

    protected virtual void Start()
    {
        if(deadPS) ParticlesManager.Instance.GetParticlePool(deadPS.name, deadPS);
    }

    Spawner mySpawner;
    public void Spawn(Transform spawnPlace, Action<Transform> returnSpawn, string _poolName, Spawner _mySpawner)
    {
        poolName = _poolName;
        myPlace = spawnPlace;
        ReturnSpawn = returnSpawn;
        transform.position = myPlace.position;
        mySpawner = _mySpawner;

    }

    public void Hit(Action<HittableBase> _removeMe)
    {
        removeMe = _removeMe;
        OnHit();
    }

    public void ReleaseHit()
    {
        OnReleaseHit();
    }

    protected void Dead()
    {
        ParticlesManager.Instance.PlayParticle(deadPS.name, transform.position + Vector3.up *3);
        removeMe?.Invoke(this);
        ReturnSpawn?.Invoke(myPlace);
        OnDead();
        if (mySpawner)
            mySpawner.ReturnHittable(poolName == null ? name : poolName, this);
        else
            Destroy(this.gameObject);
    }

    public void ExecuteErrorMesseage()
    {
        messComp.SendText(errorText);
    }

    protected abstract void OnHit();
    protected abstract void OnReleaseHit();
    protected abstract void OnDead();
    protected abstract void OnExecuteErrorMesseage();

    public abstract void OnResetHittable();

    public void ReturnObject()
    {
        ReturnSpawn?.Invoke(myPlace);
        OnResetHittable();
        HittablePool.Instance.ReturnToPool(poolName, this);
    }
}
