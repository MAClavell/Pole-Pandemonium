 using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum MenuCanvas { Main=0, Settings=1, Game=2, Pause=3, End=4, Leaderboard=5, Cosmetics }

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI endTimerText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private Toggle controlToggle;
    public Toggle ControlToggle { get; }

    [SerializeField]
    private Toggle invertToggle;
    public Toggle InvertToggle { get; }

    private GameObject[] menuCanvases;
    private MenuCanvas[] currCanvases;
    private MenuCanvas[] prevCanvases;

    private LeaderboardUI leaderBoard;

    // Start is called before the first frame update
    void Start()
    {
        menuCanvases = new GameObject[6];
        menuCanvases[0] = GameObject.Find("MainCanvas");
        menuCanvases[1] = GameObject.Find("SettingsCanvas");
        menuCanvases[2] = GameObject.Find("GameCanvas");
        menuCanvases[3] = GameObject.Find("PauseCanvas");
        menuCanvases[4] = GameObject.Find("EndCanvas");
        menuCanvases[5] = GameObject.Find("LeaderboardCanvas");
        menuCanvases[5] = GameObject.Find("CosmeticsCanvas");

        leaderBoard = menuCanvases[5].GetComponent<LeaderboardUI>();

        currCanvases = null;
        prevCanvases = null;
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Main});
    }

    /// <summary>
    /// Set a set of canvases into the active state
    /// </summary>
    /// <param name="canvases">The canvases to trigger</param>
    public void SetActiveCanvases(MenuCanvas[] canvases)
    {
        prevCanvases = currCanvases;
        for(int i = 0; i < menuCanvases.Length; i++)
        {
            if (canvases.Contains((MenuCanvas)i))
            {
                menuCanvases[i].SetActive(true);
            }
            else menuCanvases[i].SetActive(false);
        }
        currCanvases = canvases;
    }

    /// <summary>
    /// Update the game canvas's timer text
    /// </summary>
    /// <param name="time">Time to set it to</param>
    public void SetTimerText(double time)
    {
        timerText.text = $"<mspace=0.6em>{TimeSpan.FromSeconds(time).ToString("mm'.'ss'.'ff")}</mspace>";
    }

    /// <summary>
    /// Update the end canvas's timer text
    /// </summary>
    /// <param name="time">Time to set it to</param>
    public void SetEndTimerText(double time)
    {
        endTimerText.text = $"<mspace=0.6em>{TimeSpan.FromSeconds(time).ToString("mm'.'ss'.'ff")}</mspace>";

        //Set highscore text
        if(leaderBoard.IsNewHighScore)
        {
            highScoreText.text = "New highscore!";
            leaderBoard.UpdateHighScore();
        }
        else
        {
            highScoreText.text = $"Highscore - <mspace=0.6em>{TimeSpan.FromSeconds(leaderBoard.HighScore).ToString("mm'.'ss'.'ff")}</mspace>";
        }
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Show only the settings canvas
    /// </summary>
    public void ShowSettings()
    {
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Settings });
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Show the settings canvas and the game canvas
    /// </summary>
    public void ShowSettingsAndGame()
    {
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game, MenuCanvas.Settings });
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Hide the settings canvas and show the previous canvases
    /// </summary>
    public void HideSettings()
    {
        SetActiveCanvases(prevCanvases);
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for the control scheme changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnControlToggleChanged(Toggle tog)
    {
        Config.SetControlScheme(tog.isOn);
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for the inverted controls changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnInvertToggleChanged(Toggle tog)
    {
        Config.SetInvertedControls(tog.isOn);
    }
}
