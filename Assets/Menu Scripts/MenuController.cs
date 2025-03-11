using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] menuItems; 
    public RectTransform arrowIndicator; 
    private int selectedIndex = 0; 
    public AudioSource moveAudioSource;
    public AudioSource selectAudioSource;
    public AudioClip moveSound;
    public AudioClip selectSound;

    private void Start()
    {
        
        for (int i = 0; i < menuItems.Length; i++)
        {
            int index = i; 
            menuItems[i].onClick.AddListener(() => SelectMenuItem(index));
        }

        UpdateMenu(); 

        moveAudioSource = GetComponent<AudioSource>();
        selectAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayMoveSound();
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = menuItems.Length - 1;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayMoveSound();
            selectedIndex++;
            if (selectedIndex >= menuItems.Length) selectedIndex = 0;
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            PlaySelectSound();
            SelectMenuItem(selectedIndex);
        }
    }

    private void UpdateMenu()
    {
        arrowIndicator.position = new Vector3(arrowIndicator.position.x, menuItems[selectedIndex].transform.position.y, 0);
    }

    private void SelectMenuItem(int index)
    {
        switch (index)
        {
            case 0:
                Debug.Log("Play Selected!");
                SceneManager.LoadScene("GamePlay");
                break;
            case 1:
                Debug.Log("Option Selected!");
                SceneManager.LoadScene("OptionsScene");
                break;
            case 2:
                Debug.Log("Exit Selected!");
                Application.Quit();
                break;
        }
    }

    private void PlayMoveSound()
    {
        if (moveAudioSource != null && moveSound != null)
        {
            moveAudioSource.PlayOneShot(moveSound);
        }
    }

    private void PlaySelectSound()
    {
        if (selectAudioSource != null && selectSound != null)
        {
            selectAudioSource.PlayOneShot(selectSound);
        }
    }
}
