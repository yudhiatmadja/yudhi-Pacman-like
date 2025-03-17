using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinTextAnimation : MonoBehaviour
{
    private Text winText;
    private float duration = 1.5f; 
    public AudioSource winAudioSource;
    public AudioClip winSFX;
    private int playerStars;

    void Start()
    {
        winText = GetComponent<Text>();
        playerStars = PlayerPrefs.GetInt("JumlahBintang", 0);
        Debug.Log("PlayerStars: " + playerStars);
        winText.text = playerStars >= 5 ? "YOU WIN" : "GAME OVER";
        winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, 0); 
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float elapsedTime = 0;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        if (winSFX != null && winAudioSource != null)
        {
            winAudioSource.clip = winSFX;
            winAudioSource.Play();
        }

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            Color newColor = winText.color;
            newColor.a = Mathf.Lerp(0, 1, progress);
            winText.color = newColor;

            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, 1);
        transform.localScale = targetScale;
    }

    
}
