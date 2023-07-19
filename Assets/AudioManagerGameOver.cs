using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerGameOver : MonoBehaviour
{
    public static AudioManagerGameOver Instance { get; private set; }
    private AudioSource audioSource;

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
