using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitoDissappear : MonoBehaviour
{
    public ParticleSystem dissappearPS = null;
    public GameObject tito = null;

    private void Start()
    {
        ParticlesManager.Instance.GetParticlePool(dissappearPS.name, dissappearPS);
    }

    public void Execute()
    {
        ParticlesManager.Instance.PlayParticle(dissappearPS.name, transform.position);
        StartCoroutine(Couroutine());
    }

    IEnumerator Couroutine()
    {
        yield return new WaitForSeconds(0.01f);
        tito.SetActive(false);
    }
}
