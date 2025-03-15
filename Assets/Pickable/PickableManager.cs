using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableManager : MonoBehaviour
{
    private List<Pickable> _pickableList = new List<Pickable>();
    [SerializeField] private TextMeshProUGUI teksSkor;
    [SerializeField] private TextMeshProUGUI teksCoin;
    [SerializeField] private TextMeshProUGUI teksTimer;
    [SerializeField] private Player player;

    private int skorKoin = 0;
    private int jumlahKoinDiambil = 0;
    private bool isDoubleScoreActive = false;
    private int _totalKoin;
    private int _totalPickable;
    private float waktuTersisa = 90f;
    public AudioSource coinAudioSource;
    public AudioClip coinSound;
    public AudioSource menangAudioSource;
    public AudioClip menangSound;
    private bool isGameOver = false;
    // [HideInInspector] public Animator animator;


    private void Awake()
    {
        // animator = GetComponent<Animator>();
    }
    private void Start()
    {
        coinAudioSource = GetComponent<AudioSource>();
        menangAudioSource = GetComponent<AudioSource>();

        InitPickableList();
        UpdateTeksSkor();
        JumlahCoin();
        StartCoroutine(HitungMundur());
    }

    private IEnumerator HitungMundur()
    {
        while (waktuTersisa > 0)
        {
            yield return new WaitForSeconds(1f);
            waktuTersisa--;
            UpdateTeksTimer();
        }

        if (!isGameOver)
        {
            Menang(); 
        }
    }

    private void UpdateTeksTimer()
    {
        int menit = Mathf.FloorToInt(waktuTersisa / 60);
        int detik = Mathf.FloorToInt(waktuTersisa % 60);
        teksTimer.text = string.Format("{0:00}:{1:00}", menit, detik);
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
            if (coinAudioSource && coinSound)
            {
                coinAudioSource.PlayOneShot(coinSound);
            }
            UpdateTeksSkor();
            JumlahCoin();
        }
        else if (type == PickableType.PowerUp)
        {
            // Debug.Log("PowerUp diambil!");
            // animator.SetTrigger("PowerUp");
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
        if (isGameOver) return;
        isGameOver = true;

        int jumlahBintang = HitungBintang();
        Debug.Log("Jumlah Bintang yang Disimpan: " + jumlahBintang);

        PlayerPrefs.SetInt("JumlahBintang", jumlahBintang);
        PlayerPrefs.SetInt("PlayerScore", skorKoin);
        PlayerPrefs.Save();

        // LeaderboardManager.Instance.AddScore("You", skorKoin);

        StartCoroutine(LoadWinScene());
    }


    private int HitungBintang()
    {
        float persentase = (jumlahKoinDiambil / (float)_totalKoin) * 100;

        if (persentase >= 80) return 5;
        else if (persentase >= 60) return 4;
        else if (persentase >= 40) return 3;
        else if (persentase >= 20) return 2;
        else if (persentase > 0) return 1;
        return 0;
    }

    private IEnumerator LoadWinScene()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("winscene");
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
