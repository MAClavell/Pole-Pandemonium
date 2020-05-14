using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Leaderboard
{
#if UNITY_ANDROID
    private static string EASY_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAQ";
    private static string MEDIUM_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAg";
    private static string HARD_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAw";
#endif

    private static long easyHighScore = 0;
    private static long mediumHighScore = 0;
    private static long hardHighScore = 0;

    /// <summary>
    /// Init the class
    /// </summary>
    public static void Init()
    {
#if UNITY_ANDROID
        //Load highscores
        if (GooglePlayGamesController.Instance.Authenticated)
        {
            PlayGamesPlatform.Instance.LoadScores(EASY_LEADERBOARD_ID, LeaderboardStart.PlayerCentered, 
                1, LeaderboardCollection.Social, 
                LeaderboardTimeSpan.AllTime, (data) => { if (data != null) { easyHighScore = data.PlayerScore.value; } });

            PlayGamesPlatform.Instance.LoadScores(MEDIUM_LEADERBOARD_ID, LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) => { if (data != null) { mediumHighScore = data.PlayerScore.value; } });

            PlayGamesPlatform.Instance.LoadScores(HARD_LEADERBOARD_ID, LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) => { if (data != null) { hardHighScore = data.PlayerScore.value; } });
        }
#endif
    }

    /// <summary>
    /// Update the high score with the current game time
    /// </summary>
    /// <returns>Whether the current score is a new highscore</returns>
    public static void UpdateHighScore()
    {
        long time = (long)(GameManager.Instance.GameTime * 1000);
        bool isNewHighScore = false;
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
#if UNITY_ANDROID
                if (GooglePlayGamesController.Instance.Authenticated)
                {
                    PlayGamesPlatform.Instance.ReportScore(time, EASY_LEADERBOARD_ID, (success) => { });
                }
#endif

                if (time > easyHighScore)
                {
                    easyHighScore = time;
                    isNewHighScore = true;
                }
                break;

            case Difficulty.Medium:
#if UNITY_ANDROID
                if (GooglePlayGamesController.Instance.Authenticated)
                {
                    PlayGamesPlatform.Instance.ReportScore(time, MEDIUM_LEADERBOARD_ID, (success) => { });
                }
#endif

                if (time > mediumHighScore)
                {
                    mediumHighScore = time;
                    isNewHighScore = true;
                }
                break;

            case Difficulty.Hard:
#if UNITY_ANDROID
                if (GooglePlayGamesController.Instance.Authenticated)
                {
                    PlayGamesPlatform.Instance.ReportScore(time, HARD_LEADERBOARD_ID, (success) => { });
                }
#endif

                if (time > hardHighScore)
                {
                    hardHighScore = time;
                    isNewHighScore = true;
                }
                break;
        }

        //Update the text
        GameManager.Instance.MenuManager.SetHighScoreText(isNewHighScore);
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

}
