using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();
    [SerializeField] private TextMeshProUGUI teksSkor;
    [SerializeField] private TextMeshProUGUI teksCoin;
    [SerializeField] private Player player;
    [SerializeField] private GameObject panelMenang;

    private int skorKoin = 0;
    private int jumlahKoinDiambil = 0; 
    private bool isDoubleScoreActive = false;
    private int _totalKoin; 
    private int _totalPickable;
    public AudioSource audioSource; 
    public AudioClip coinSound;

    private void Start()
    {
        panelMenang.SetActive(false);
        InitPickableList();
        UpdateTeksSkor();
        JumlahCoin();
    }

    private void InitPickableList()
    {
        Pickable[] pickableObjects = GameObject.FindObjectsByType<Pickable>(FindObjectsSortMode.None);

        foreach (Pickable pickable in pickableObjects)
        {
            _pickableList.Add(pickable);
            pickable.OnPicked += (type, p) => OnPickablePicked(type, p);
        }

        _totalPickable = _pickableList.Count;

        
        _totalKoin = 0;
        foreach (Pickable pickable in _pickableList)
        {
            if (pickable.GetPickableType() == PickableType.Coin)
            {
                _totalKoin++;
            }
        }

        // Debug.Log($"Total Pickable: {_totalPickable}, Total Koin: {_totalKoin}");
    }

    private void OnPickablePicked(PickableType type, Pickable pickable)
    {
        _pickableList.Remove(pickable);
        Destroy(pickable.gameObject);

        if (type == PickableType.Coin)
        {
            jumlahKoinDiambil++; 
            int nilaiKoin = isDoubleScoreActive ? 2 : 1;
            skorKoin += nilaiKoin;
            if (audioSource && coinSound)
            {
                audioSource.PlayOneShot(coinSound);
            }
            UpdateTeksSkor();
            JumlahCoin();
        }
        else if (type == PickableType.PowerUp)
        {
            // Debug.Log("PowerUp diambil!");
            player?.PickPowerUp();
            StartCoroutine(AktifkanPowerUp());
        }

        if (jumlahKoinDiambil >= _totalKoin) 
        {
            Menang();
        }
    }

    private void UpdateTeksSkor()
    {
        teksSkor.text = "Score: " + skorKoin;
    }

    private void JumlahCoin()
    {
        teksCoin.text = "Coin: " + jumlahKoinDiambil + "/" + _totalKoin;
    }
    private void Menang()
    {
        panelMenang.SetActive(true);
        Time.timeScale = 0f;
    }

    private IEnumerator AktifkanPowerUp()
    {
        // Debug.Log("PowerUp Aktif: Magnet + Double Score (5 detik)");

        player.AktifkanMagnet(true);
        isDoubleScoreActive = true;

        yield return new WaitForSeconds(5);

        player.AktifkanMagnet(false);
        isDoubleScoreActive = false;

        // Debug.Log("PowerUp selesai");
    }
}
