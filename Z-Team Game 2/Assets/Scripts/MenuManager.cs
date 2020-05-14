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
    public static readonly Color DEFAULT_FILTER_IN_COLOR = new Color(0, 0, 0, 0.3f);
    public static readonly Color DEFAULT_FILTER_OUT_COLOR = new Color(0, 0, 0, 0);

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI endTimerText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private RectTransform filter;
    [SerializeField]
    private RectTransform fadeIn;

    /// <summary>
    /// The current set of active canvases
    /// </summary>
    public MenuCanvas[] CurrCanvases { get; private set; }
    /// <summary>
    /// The previous set of active canvases
    /// </summary>
    public MenuCanvas[] PrevCanvases { get; private set; }
    private IMenuUIBase[] menuCanvases;

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

        CurrCanvases = null;
        PrevCanvases = null;
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Main});
        fadeIn.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        TweenColor(fadeIn, DEFAULT_FILTER_OUT_COLOR, 0.75f).setOnComplete(() => Destroy(fadeIn.gameObject));
    }

    /// <summary>
    /// Set a set of canvases into the active state
    /// </summary>
    /// <param name="canvases">The canvases to trigger</param>
    public void SetActiveCanvases(MenuCanvas[] canvases)
    {
        PrevCanvases = CurrCanvases;
        CurrCanvases = canvases;
        for(int i = 0; i < menuCanvases.Length; i++)
        {
            if (canvases.Contains((MenuCanvas)i))
            {
                if(!menuCanvases[i].Active)
                    menuCanvases[i].Activate();
            }
            else
            {
                if(menuCanvases[i].Active)
                    menuCanvases[i].Deactivate();
            }
        }
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
    }

    /// <summary>
    /// Set the highscore text
    /// </summary>
    /// <param name="isNewHighScore">Whether there is a new highscore or not</param>
    public void SetHighScoreText(bool isNewHighScore)
    {
        //Set highscore text
        if (isNewHighScore)
        {
            highScoreText.text = "New highscore!";
        }
        else
        {
            highScoreText.text = $"Highscore - <mspace=0.6em>{TimeSpan.FromMilliseconds(Leaderboard.GetCurrentHighScore()).ToString("mm'.'ss'.'ff")}</mspace>";
        }
    }

    /// <summary>
    /// Set the filter object to a certain color
    /// </summary>
    /// <param name="newColor">Color to set</param>
    /// <param name="time">How long the transition should take</param>
    public void SetFilterColor(Color newColor, float time)
    {
        TweenColor(filter, newColor, time);
    }

    /// <summary>
    /// Tween the color of a rectransform image's color
    /// </summary>
    /// <param name="image">Rectransform to tween</param>
    /// <param name="newColor">New final color</param>
    /// <param name="time">How long the tween should take</param>
    private LTDescr TweenColor(RectTransform image, Color newColor, float time)
    {
        LeanTween.cancel(image);
        return LeanTween.color(image, newColor, time).setIgnoreTimeScale(true);
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Hide the current canvases and show cosmetics canvas
    /// </summary>
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
        SetActiveCanvases(PrevCanvases);
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
        SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Settings, MenuCanvas.Game });
    }

    /// <summary>
    /// Set the gravity scale UI to a number
    /// </summary>
    /// <param name="gravityScale">New scale</param>
    public void SetGravityScaleUI(float gravityScale)
    {
        ((GameUI)menuCanvases[(int)MenuCanvas.Game]).UpdateGravityText(gravityScale);
    }

    /// <summary>
    /// Show the leaderboard UI
    /// </summary>
    public void ShowLeaderboard()
    {
#if UNITY_ANDROID
        GooglePlayGamesController.Instance.ShowLeaderboardUI();
#endif
    }
}
