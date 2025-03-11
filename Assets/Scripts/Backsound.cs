using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Backsound : MonoBehaviour
{
    public static Backsound instance { get; private set; }

    private AudioSource audioSource;
    private AudioSource readyAudioSource;
    public AudioClip readySound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        readyAudioSource = gameObject.AddComponent<AudioSource>();

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            StartCoroutine(PlaySFXthenMusic()); 
        }
        else
        {
            StopMusic(); 
        }
    }

    private IEnumerator PlaySFXthenMusic()
    {
        if (readySound != null)
        {
            readyAudioSource.clip = readySound;
            readyAudioSource.Play();
            yield return new WaitForSeconds(readySound.length);
        }
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }


    public void StopMusicOnPlayerDeath()
    {
        StopMusic();
    }
}
