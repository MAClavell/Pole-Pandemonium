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
    private const string EASY_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAQ";
    private const string MEDIUM_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAg";
    private const string HARD_LEADERBOARD_ID = "CgkI15XZ3ZoKEAIQAw";
#endif

    private const string EASY_SCORE_PP_KEY = "EasyScore";
    private const string MEDIUM_SCORE_PP_KEY = "MediumScore";
    private const string HARD_SCORE_PP_KEY = "HardScore";

    private static long easyHighScore = 0;
    private static long mediumHighScore = 0;
    private static long hardHighScore = 0;

    /// <summary>
    /// Load the scores from disk
    /// </summary>
    public static void Init()
    {
        LoadHighScoresFromDisk();
    }

    /// <summary>
    /// Load the scores from the game's service
    /// </summary>
    public static void InitLeaderboard()
    {
        LoadHighScoresFromGameServce();
    }

    /// <summary>
    /// Load our highscores from the disk
    /// </summary>
    private static void LoadHighScoresFromDisk()
    {
        //Read from disc first
        if (PlayerPrefs.HasKey(EASY_SCORE_PP_KEY))
            long.TryParse(Base64Decode(PlayerPrefs.GetString(EASY_SCORE_PP_KEY)), out easyHighScore);
        if (PlayerPrefs.HasKey(MEDIUM_SCORE_PP_KEY))
            long.TryParse(Base64Decode(PlayerPrefs.GetString(MEDIUM_SCORE_PP_KEY)), out mediumHighScore);
        if (PlayerPrefs.HasKey(HARD_SCORE_PP_KEY))
            long.TryParse(Base64Decode(PlayerPrefs.GetString(HARD_SCORE_PP_KEY)), out hardHighScore);
    }

    /// <summary>
    /// Load our highscores from the leaderboard and compare them to the one's read
    /// </summary>
    private static void LoadHighScoresFromGameServce()
    { 
        //Load highscores from leaderboard and compare to our disk's scores
        if (GamesServicesController.Authenticated)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.LoadScores(EASY_LEADERBOARD_ID, LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) =>
                {
                    if (data != null && data.PlayerScore.value > easyHighScore)
                    {
                        easyHighScore = data.PlayerScore.value;
                    }
                });

            PlayGamesPlatform.Instance.LoadScores(MEDIUM_LEADERBOARD_ID, LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) =>
                {
                    if (data != null && data.PlayerScore.value > mediumHighScore)
                    {
                        mediumHighScore = data.PlayerScore.value;
                    }
                });

            PlayGamesPlatform.Instance.LoadScores(HARD_LEADERBOARD_ID, LeaderboardStart.PlayerCentered,
                1, LeaderboardCollection.Social,
                LeaderboardTimeSpan.AllTime, (data) =>
                {
                    if (data != null && data.PlayerScore.value > hardHighScore)
                    {
                        hardHighScore = data.PlayerScore.value;
                    }
                });
#endif
        }
    }

    /// <summary>
    /// Save the highscore for a specific difficulty
    /// </summary>
    /// <param name="diff"></param>
    /// <returns></returns>
    private static bool SaveHighScore(Difficulty diff)
    {
        //Recalculate time for security
        long time = (long)(GameManager.Instance.GameTime * 1000);

        //Send highscore to the games service
        if (GamesServicesController.Authenticated)
        {
            switch (diff)
            {
                case Difficulty.Easy:
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.ReportScore(time, EASY_LEADERBOARD_ID, (success) => { });
#endif
                    break;

                case Difficulty.Medium:
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.ReportScore(time, MEDIUM_LEADERBOARD_ID, (success) => { });
#endif
                    break;

                case Difficulty.Hard:
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.ReportScore(time, HARD_LEADERBOARD_ID, (success) => { });
#endif
                    break;

                default:
                    break;
            }
        }

        switch (diff)
        {
            case Difficulty.Easy when time > easyHighScore:
                easyHighScore = time;
                PlayerPrefs.SetString(EASY_SCORE_PP_KEY, Base64Encode(easyHighScore.ToString()));
                PlayerPrefs.Save();
                return true;

            case Difficulty.Medium when time > mediumHighScore:
                mediumHighScore = time;
                PlayerPrefs.SetString(MEDIUM_SCORE_PP_KEY, Base64Encode(mediumHighScore.ToString()));
                PlayerPrefs.Save();
                return true;

            case Difficulty.Hard when time > hardHighScore:
                hardHighScore = time;
                PlayerPrefs.SetString(HARD_SCORE_PP_KEY, Base64Encode(hardHighScore.ToString()));
                PlayerPrefs.Save();
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Update the high score with the current game time
    /// </summary>
    /// <returns>Whether the current score is a new highscore</returns>
    public static void UpdateHighScore()
    {
        bool isNewHighScore = false;
        switch (Config.Difficulty)
        {
            case Difficulty.Easy:
                isNewHighScore = SaveHighScore(Difficulty.Easy);
                break;

            case Difficulty.Medium:
                isNewHighScore = SaveHighScore(Difficulty.Medium);
                break;

            case Difficulty.Hard:
                isNewHighScore = SaveHighScore(Difficulty.Hard);
                break;

            default:
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

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

}
