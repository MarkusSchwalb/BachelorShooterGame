using UnityEngine;
using static VolFx.OldMoviePass;

public class CreateAudioOnStart : MonoBehaviour
{
    public AudioClip audioClip;
    public GameObject AudioO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioClip == null && AudioO == null) return;
        
        GameObject go = Instantiate(AudioO, transform.position, transform.rotation);
        AudioObject audioObject = go.GetComponent<AudioObject>();
        if (audioObject != null)
        {
            audioObject.SoundClip = audioClip;
            audioObject.playSound();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
