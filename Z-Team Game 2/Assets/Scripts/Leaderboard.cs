using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Leaderboard
{
    private const string LEADERBOARD_PATH = "/Leaderboard.pole";

    /// <summary>
    /// Class for containing highscore data for each difficulty
    /// </summary>
    [System.Serializable]
    private class HighScoreEntries
    {
        public LeaderboardEntry easy;
        public LeaderboardEntry medium;
        public LeaderboardEntry hard;
    }

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

    private static HighScoreEntries highScores;
   
    /// <summary>
    /// Init the class
    /// </summary>
    public static void Init()
    {
        //If there is an error loading, delete the file and load defaults
        try
        {
            LoadScore();
        }
        catch when (File.Exists(Application.persistentDataPath + LEADERBOARD_PATH))
        {
            File.Delete(Application.persistentDataPath + LEADERBOARD_PATH);
            LoadScore();
        }
    }

    /// <summary>
    /// Save the current game time as the new highscore
    /// </summary>
    private static void SaveScore()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + LEADERBOARD_PATH, false))
        {
            bf.Serialize(sw.BaseStream, highScores);
        }
    }

    /// <summary>
    /// Load the player's highest score
    /// </summary>
    private static void LoadScore()
    {
        //New highscore
        if (!File.Exists(Application.persistentDataPath + LEADERBOARD_PATH) ||
            string.IsNullOrWhiteSpace(Application.persistentDataPath + LEADERBOARD_PATH))
        {
            highScores = new HighScoreEntries();
            highScores.easy = new LeaderboardEntry(0);
            highScores.medium = new LeaderboardEntry(0);
            highScores.hard = new LeaderboardEntry(0);
            return;
        }

        //Load old highscore
        BinaryFormatter bf = new BinaryFormatter();
        using (StreamReader sw = new StreamReader(Application.persistentDataPath + LEADERBOARD_PATH))
        {
            highScores = (HighScoreEntries)bf.Deserialize(sw.BaseStream);
        }
    }

    /// <summary>
    /// Update the high score with the current game time
    /// </summary>
    public static void UpdateHighScore()
    {
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                highScores.easy = new LeaderboardEntry(GameManager.Instance.GameTime);
                break;

            case Difficulty.Medium:
                highScores.medium = new LeaderboardEntry(GameManager.Instance.GameTime);
                break;

            case Difficulty.Hard:
                highScores.hard = new LeaderboardEntry(GameManager.Instance.GameTime);
                break;
        }
        SaveScore();
    }

    /// <summary>
    /// Get the current highscore
    /// </summary>
    /// <returns></returns>
    public static double GetCurrentHighScore()
    {
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                return highScores.easy.Time;

            case Difficulty.Medium:
                return highScores.medium.Time;

            case Difficulty.Hard:
                return highScores.hard.Time;
        }
        return 0;
    }

    /// <summary>
    /// If our current game time is the newest highscore
    /// </summary>
    public static bool IsNewHighScore()
    {
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                return highScores.easy.Time < GameManager.Instance.GameTime;

            case Difficulty.Medium:
                return highScores.medium.Time < GameManager.Instance.GameTime;

            case Difficulty.Hard:
                return highScores.hard.Time < GameManager.Instance.GameTime;
        }
        return false;
    }
}
