using UnityEngine;
using UnityEngine.SceneManagement;

public class Backsound : MonoBehaviour
{
    public static Backsound instance { get; private set; }

    private AudioSource audioSource;

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
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            PlayMusic(); 
        }
        else
        {
            StopMusic(); 
        }
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
