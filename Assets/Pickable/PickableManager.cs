using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();
    [SerializeField] private TextMeshProUGUI teksSkor;
    [SerializeField] private Player player;
    private int skorKoin = 0;
    private bool isDoubleScoreActive = false;


    private void Start()
    {
        InitPickableList();
        UpdateTeksSkor();
    }

    private void InitPickableList()
    {
        Pickable[] pickableObjects = GameObject.FindObjectsByType<Pickable>(FindObjectsSortMode.None);

        for (int i = 0; i < pickableObjects.Length; i++)
        {
            _pickableList.Add(pickableObjects[i]);
            pickableObjects[i].OnPicked += (type, pickable) => OnPickablePicked(type, pickable);
        }


        Debug.Log("Jumlah Pickable: " + _pickableList.Count);
    }

    private void OnPickablePicked(PickableType type, Pickable pickable)
    {
        _pickableList.Remove(pickable);
        Destroy(pickable.gameObject);

        if (type == PickableType.Coin)
        {
            int nilaiKoin = isDoubleScoreActive ? 2 : 1;  // Double score aktif atau tidak
            skorKoin += nilaiKoin;
            UpdateTeksSkor();
        }
        else if (type == PickableType.PowerUp)
        {
            Debug.Log("PowerUp diambil!");
            player?.PickPowerUp();
            StartCoroutine(AktifkanPowerUp());
        }

        if (_pickableList.Count <= 0)
        {
            Debug.Log("Menang! Semua item sudah diambil.");
        }
    }


    private void UpdateTeksSkor()
    {
        teksSkor.text = "Koin: " + skorKoin;
    }

    private IEnumerator AktifkanPowerUp()
    {
        Debug.Log("PowerUp Aktif: Magnet + Double Score (5 detik)");

        // Aktifkan efek magnet di player
        player.AktifkanMagnet(true);

        // Aktifkan double score
        isDoubleScoreActive = true;

        yield return new WaitForSeconds(5);

        // Matikan efek setelah 5 detik
        player.AktifkanMagnet(false);
        isDoubleScoreActive = false;

        Debug.Log("PowerUp selesai");
    }
}
