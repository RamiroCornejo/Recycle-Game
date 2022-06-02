using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] float timeOutside = 15;
    float timer;

    public UnityEvent OnRespawn;

    void Start()
    {
        
    }

    void Update()
    {
        if(timer < timeOutside)
            timer += Time.deltaTime;
    }

    public void Spawn()
    {
        if (timer >= timeOutside)
            OnRespawn.Invoke();

        timer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Character>();

        if (player == null) return;
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<Character>();

        if (player == null) return;
        timer = 0;
    }
}
