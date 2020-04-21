﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardUI : MonoBehaviour
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

    private HighScoreEntries highScores;
   
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
            bf.Serialize(sw.BaseStream, highScores);
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
    public void UpdateHighScore()
    {
        highScores.medium = new LeaderboardEntry(GameManager.Instance.GameTime);
        SaveScore();
    }

    public double GetHighScore()
    {
        return highScores.medium.Time;
    }

    /// <summary>
    /// If our current game time is the newest highscore
    /// </summary>
    public bool IsNewHighScore()
    {
        return highScores.medium.Time < GameManager.Instance.GameTime;
    }

    public void OnGlobalFriendSelectionChanged(Button prev, Button curr)
    {

    }

    public void OnDifficultySelectionChanged(Button prev, Button curr)
    {

    }
}