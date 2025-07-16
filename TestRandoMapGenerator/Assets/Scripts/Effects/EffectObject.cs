using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public GameObject ParticleObj;
    public ParticleSystem PS;
    GameObject spawnedParticle;
    public void PlayEffect()
    {
        if (ParticleObj == null) { return; }
        spawnedParticle =  Instantiate(ParticleObj, gameObject.transform);

        PS = spawnedParticle.GetComponent<ParticleSystem>();

        if (PS == null)
        {
            Destroy(gameObject);
            return;
        }

        PS.Play();
        float duration = PS.main.duration + PS.main.startLifetime.constant;
        StartCoroutine(DestroyAfterEffect(duration));
    }

    IEnumerator DestroyAfterEffect(float time)
    {
        yield return new WaitForSeconds(time);
        //UnityEngine.Debug.Log("Delete");
        Destroy(spawnedParticle);
        Destroy(gameObject);
    }
}
