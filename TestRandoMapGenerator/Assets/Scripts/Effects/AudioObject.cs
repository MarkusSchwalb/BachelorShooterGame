using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioObject : MonoBehaviour
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
        if (SoundClip == null) Destroy(gameObject);
        StartCoroutine(DestroyAfterSound(SoundClip.length));
    }

    IEnumerator DestroyAfterSound(float time)
    {
        yield return new WaitForSeconds(time);
        //UnityEngine.Debug.Log("Delete");
        Destroy(gameObject);
    }
}
