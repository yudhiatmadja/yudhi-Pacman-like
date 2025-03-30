using System;
using System.Collections;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource _audioSource;
    
    public AudioClip coinSound;
    public AudioClip enemyHurt;
    public AudioClip playerHurt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "playerHurt":
                _audioSource.PlayOneShot(playerHurt);
                break;
            case "enemyHurt":
                _audioSource.PlayOneShot(enemyHurt);
                break;
            case "coin":
                _audioSource.PlayOneShot(coinSound);
                break;
        }
    }
}