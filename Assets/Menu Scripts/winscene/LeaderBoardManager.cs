using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private Text leaderboardText;
    public static LeaderboardManager Instance;
    private List<PlayerData> leaderboard = new List<PlayerData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("Leaderboard_Count");
        PlayerPrefs.Save();

        leaderboard.Clear();
        LoadLeaderboard();
        AddDummyPlayers();

        if (leaderboard.Count == 0)
        {
            AddDummyPlayers();
        }

        int playerScore = PlayerPrefs.GetInt("PlayerScore", -1); // Set default ke -1 untuk mengecek jika tidak ada
        string playerName = PlayerPrefs.GetString("PlayerName", "You");



        if (playerScore != -1)
        {
            AddScore(playerName, playerScore);
        }

         UpdateLeaderboardUI();
    }


    private void AddDummyPlayers()
    {
        leaderboard.RemoveAll(player => player.isDummy);

        string[] randomNames = { "Rex", "Shadow", "Blaze", "Knight", "Ghost", "Falcon", "Titan", "Nova", "Storm", "Hunter" };

       while (leaderboard.Count < 4)
    {
        string randomName = randomNames[Random.Range(0, randomNames.Length)];
        int randomScore = Random.Range(10, 100); 
        leaderboard.Add(new PlayerData(randomName, randomScore, true)); 
    }


    SaveLeaderboard(); 
    UpdateLeaderboardUI();
    }


    public void AddScore(string playerName, int score)
    {
        leaderboard.RemoveAll(player => player.name == playerName);

        leaderboard.Add(new PlayerData(playerName, score));
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        // Debug.Log("Leaderboard setelah ditambah:");
        // foreach (var player in leaderboard)
        // {
        //     Debug.Log($"{player.name} - {player.score}");
        // }

        SaveLeaderboard();
        UpdateLeaderboardUI(); // <-- Pastikan ini dipanggil
    }


    private void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            PlayerPrefs.SetString($"Leaderboard_Name_{i}", leaderboard[i].name);
            PlayerPrefs.SetInt($"Leaderboard_Score_{i}", leaderboard[i].score);
        }
        PlayerPrefs.SetInt("Leaderboard_Count", leaderboard.Count);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        leaderboard.Clear();
        int count = PlayerPrefs.GetInt("Leaderboard_Count", 0);

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"Leaderboard_Name_{i}", $"Player_{i + 1}");
            int score = PlayerPrefs.GetInt($"Leaderboard_Score_{i}", 0);
            leaderboard.Add(new PlayerData(name, score));
        }

        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI()
    {
        if (leaderboardText != null)
        {
            leaderboardText.text = "Leaderboard:\n";

            if (leaderboard.Count == 0)
            {
                Debug.LogWarning("Leaderboard kosong saat update UI!");
                return;
            }

            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboardText.text += $"{i + 1}. {leaderboard[i].name} - {leaderboard[i].score}\n";
            }

            Debug.Log($"Leaderboard text updated: {leaderboardText.text}");
        }
        else
        {
            Debug.LogError("leaderboardText belum diassign di Inspector!");
        }
    }


}

public class PlayerData
{
    public string name;
    public int score;
    public bool isDummy;

    public PlayerData(string name, int score, bool isDummy = false)
    {
        this.name = name;
        this.score = score;
        this.isDummy = isDummy;
    }
}
