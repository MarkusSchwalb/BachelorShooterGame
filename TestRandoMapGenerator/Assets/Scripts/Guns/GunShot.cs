using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GunShot : MonoBehaviour
{
    public AudioSource AudioSource;    
    public AudioClip SoundClip;

    bool played = false;

    public void playSound()
    {
        //UnityEngine.Debug.Log("play sound");
        if (AudioSource == null) AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = SoundClip;
        AudioSource.Play();
        StartCoroutine(DestroyAfterSound(SoundClip.length));
    }

    IEnumerator DestroyAfterSound(float time)
    {
        yield return new WaitForSeconds(time);
        //UnityEngine.Debug.Log("Delete");
        Destroy(gameObject);
    }
}
