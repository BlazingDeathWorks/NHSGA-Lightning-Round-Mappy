using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerNextLevel : MonoBehaviour
{
    public static AudioManagerNextLevel Instance { get; private set; }
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
        
    }

    
}
