using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthManager : MonoBehaviour
{
    public static UIHealthManager Instance;

    [SerializeField] private List<Image> heartImages; 
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart; 
            }
            else
            {
                heartImages[i].sprite = emptyHeart; 
            }
        }
    }
}
