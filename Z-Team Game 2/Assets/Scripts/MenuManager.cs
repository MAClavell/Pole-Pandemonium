 using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum MenuCanvas { Main=0, Settings=1, Game=2, Pause=3, End=4, Cosmetics=5 }

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

    private IMenuUIBase[] menuCanvases;
    private MenuCanvas[] currCanvases;
    private MenuCanvas[] prevCanvases;

    // Start is called before the first frame update
    void Start()
    {
        menuCanvases = new IMenuUIBase[System.Enum.GetValues(typeof(MenuCanvas)).Length];
        menuCanvases[(int)MenuCanvas.Main] = GameObject.Find("MainCanvas").GetComponent<IMenuUIBase>();
        menuCanvases[(int)MenuCanvas.Settings] = GameObject.Find("SettingsCanvas").GetComponent<IMenuUIBase>();
        menuCanvases[(int)MenuCanvas.Game] = GameObject.Find("GameCanvas").GetComponent<IMenuUIBase>();
        menuCanvases[(int)MenuCanvas.Pause] = GameObject.Find("PauseCanvas").GetComponent<IMenuUIBase>();
        menuCanvases[(int)MenuCanvas.End] = GameObject.Find("EndCanvas").GetComponent<IMenuUIBase>();
        menuCanvases[(int)MenuCanvas.Cosmetics] = GameObject.Find("CosmeticsCanvas").GetComponent<IMenuUIBase>();

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
                bool previouslyActive = menuCanvases[i].GameObject.activeInHierarchy;
                menuCanvases[i].GameObject.SetActive(true);
                menuCanvases[i].Activate(previouslyActive);
            }
            else
            {
                bool previouslyActive = menuCanvases[i].GameObject.activeInHierarchy;
                menuCanvases[i].GameObject.SetActive(false);
                menuCanvases[i].Deactivate(previouslyActive);
            }
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
        if(Leaderboard.IsNewHighScore())
        {
            highScoreText.text = "New highscore!";
            Leaderboard.UpdateHighScore();
        }
        else
        {
            highScoreText.text = $"Highscore - <mspace=0.6em>{TimeSpan.FromSeconds(Leaderboard.GetCurrentHighScore()).ToString("mm'.'ss'.'ff")}</mspace>";
        }
    }

    public void ShowCosmetics()
    {
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Cosmetics });
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Hide the current canvases and show the previous canvases
    /// </summary>
    public void ShowPreviousCanvases()
    {
        SetActiveCanvases(prevCanvases);
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
}
