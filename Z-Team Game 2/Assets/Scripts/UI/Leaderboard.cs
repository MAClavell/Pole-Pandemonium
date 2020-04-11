using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Leaderboard : MonoBehaviour
{
    private const string LEADERBOARD_PATH = "/Leaderboard.pole";

    /// <summary>
    /// Class for containing leaderboard entry data
    /// </summary>
    [System.Serializable]
    private class LeaderboardEntry
    {
        public double Time { get; private set; }
        public LeaderboardEntry(double time)
        {
            Time = time;
        }
    }

    /// <summary>
    /// Get the player's current high score
    /// </summary>
    public double HighScore { get { return highScore.Time; } }
    private LeaderboardEntry highScore;

    /// <summary>
    /// If our current game time is the newest highscore
    /// </summary>
    public bool IsNewHighScore { get { return highScore.Time < GameManager.Instance.GameTime; } }
   
    /// <summary>
    /// Constructor
    /// </summary>
    private void Awake()
    {
        LoadScore();
    }

    /// <summary>
    /// Save the current game time as the new highscore
    /// </summary>
    private void SaveScore()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + LEADERBOARD_PATH, false))
        {
            bf.Serialize(sw.BaseStream, highScore);
        }
    }

    /// <summary>
    /// Load the player's highest score
    /// </summary>
    private void LoadScore()
    {
        //New highscore
        if (!File.Exists(Application.persistentDataPath + LEADERBOARD_PATH) ||
            string.IsNullOrWhiteSpace(Application.persistentDataPath + LEADERBOARD_PATH))
        {
            highScore = new LeaderboardEntry(0);
            return;
        }

        //Load old highscore
        BinaryFormatter bf = new BinaryFormatter();
        using (StreamReader sw = new StreamReader(Application.persistentDataPath + LEADERBOARD_PATH))
        {
            highScore = (LeaderboardEntry)bf.Deserialize(sw.BaseStream);
        }
    }

    /// <summary>
    /// Update the high score with the current game time
    /// </summary>
    public void UpdateHighScore()
    {
        highScore = new LeaderboardEntry(GameManager.Instance.GameTime);
        SaveScore();
    }

    public void OnGlobalFriendSelectionChanged(Button prev, Button curr)
    {

    }

    public void OnDifficultySelectionChanged(Button prev, Button curr)
    {

    }
}
