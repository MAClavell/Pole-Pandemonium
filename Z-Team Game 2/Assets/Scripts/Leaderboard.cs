﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if !UNITY_EDITOR && UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Leaderboard
{
#if !UNITY_EDITOR && UNITY_ANDROID
    private static string EASY_LEADERBOARD_ID = "CgkIwq22wv0HEAIQAw";
    private static string MEDIUM_LEADERBOARD_ID = "CgkIwq22wv0HEAIQAg";
    private static string HARD_LEADERBOARD_ID = "CgkIwq22wv0HEAIQBA";
#endif

    private static long easyHighScore = 0;
    private static long mediumHighScore = 0;
    private static long hardHighScore = 0;

    /// <summary>
    /// Init the class
    /// </summary>
    public static void Init()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        //Load highscores
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.LoadScores("", LeaderboardStart.PlayerCentered, 
                1, LeaderboardCollection.Social, 
                LeaderboardTimeSpan.AllTime, (data) => { easyHighScore = data.PlayerScore.value; });

            PlayGamesPlatform.Instance.LoadScores("", LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) => { mediumHighScore = data.PlayerScore.value; });

            PlayGamesPlatform.Instance.LoadScores("", LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) => { hardHighScore = data.PlayerScore.value; });
        }
#endif
    }

    /// <summary>
    /// Update the high score with the current game time
    /// </summary>
    public static void UpdateHighScore()
    {
        long time = (long)(GameManager.Instance.GameTime * 1000);
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                easyHighScore = time;
#if !UNITY_EDITOR && UNITY_ANDROID
                if(Social.localUser.authenticated)
                    Social.ReportScore(time, EASY_LEADERBOARD_ID, (success) => { });
#endif
                break;

            case Difficulty.Medium:
                mediumHighScore = time;
#if !UNITY_EDITOR && UNITY_ANDROID
                if(Social.localUser.authenticated)
                    Social.ReportScore(time, MEDIUM_LEADERBOARD_ID, (success) => { });
#endif
                break;

            case Difficulty.Hard:
                hardHighScore = time;
#if !UNITY_EDITOR && UNITY_ANDROID
                if(Social.localUser.authenticated)
                    Social.ReportScore(time, HARD_LEADERBOARD_ID, (success) => { });
#endif
                break;
        }
    }

    /// <summary>
    /// Get the current highscore
    /// </summary>
    /// <returns></returns>
    public static long GetCurrentHighScore()
    {
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                return easyHighScore;

            case Difficulty.Medium:
                return mediumHighScore;

            case Difficulty.Hard:
                return hardHighScore;
        }
        return 0;
    }

    /// <summary>
    /// If our current game time is the newest highscore
    /// </summary>
    public static bool IsNewHighScore()
    {
        long time = (long)(GameManager.Instance.GameTime * 1000);
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                return easyHighScore < time;

            case Difficulty.Medium:
                return mediumHighScore < time;

            case Difficulty.Hard:
                return hardHighScore < time;
        }
        return false;
    }
}
