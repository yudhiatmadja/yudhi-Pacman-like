using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] bintang;
    [SerializeField] private Sprite emptyStarSprite;
    [SerializeField] private Text scoreText;
    // public Button replayBtn;
    // public ButtonManagement mainMenuBtn;
    private string gameplaySceneName = "Gameplay";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int jumlahBintang = PlayerPrefs.GetInt("JumlahBintang", 0);
        int score = PlayerPrefs.GetInt("PlayerScore", 0);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("scoreText belum diassign di Inspector!");
        }

        for (int i = 0; i < bintang.Length; i++)
        {
            if (i < jumlahBintang)
            {
                bintang[i].SetActive(true);
                Debug.Log($"Mengaktifkan bintang penuh ke-{i}");
            }
            else
            {
                Image starImage = bintang[i].GetComponent<Image>();
                if (starImage != null && emptyStarSprite != null)
                {
                    starImage.sprite = emptyStarSprite;
                }
                Debug.Log($"Mengubah bintang ke-{i} menjadi kosong");
            }
        }
    }

    public void ReplayGame()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
