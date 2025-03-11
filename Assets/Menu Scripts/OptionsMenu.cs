using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; 
    public Text volumeText;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeSlider.value = savedVolume;
        if (Backsound.instance != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
            Backsound.instance.SetVolume(volumeSlider.value);
        }

        UpdateVolumeText(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (Backsound.instance != null)
        {
            Backsound.instance.SetVolume(volume);
            PlayerPrefs.SetFloat("Volume", volume); 
        }

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        UpdateVolumeText(volume);
    }

    private void UpdateVolumeText(float volume)
    {
        volumeText.text = "(" + Mathf.Round(volume * 100) + "%)";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}
