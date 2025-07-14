using UnityEngine;

public class BackgrounMusik : MonoBehaviour
{
    [field: SerializeField] private AudioClip musicClip;
    [field: SerializeField] private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioSource == null)
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
