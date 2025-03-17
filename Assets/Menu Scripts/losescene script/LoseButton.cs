using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseButton : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ReplayGame()
    {
        PlayerPrefs.SetInt("PlayerScore", 0); 
        PlayerPrefs.Save(); 
        SceneManager.LoadScene("GamePlay");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}
