using Crystal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : Singleton<GameManager>
{
    // The force due to gravity
    public const float GRAVITY = 40f;
    private const float LOSING_ROTATION = 89.9f;

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentState { get; private set; }
    /// <summary>
    /// Whether the game is currenly being played
    /// </summary>
    public bool IsPlaying { get => CurrentState == GameState.Playing; }

    [SerializeField]
    private Pole pole;
    [SerializeField]
    private MenuManager menuManager;

    private double totalTime;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentState = GameState.MainMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        SafeArea.Sim = SafeArea.SimDevice.Pixel3XL_LSR;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case GameState.MainMenu:
                break;

            case GameState.Playing:
                pole.OnUpdate();

                //Update UI timer
                totalTime += Time.deltaTime;
                menuManager.SetTimerText(totalTime);

                if (Mathf.Abs(pole.Rotation) > LOSING_ROTATION)
                    EndGame();
                break;
            
            case GameState.Paused:
                break;
            
            case GameState.GameOver:
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// Reset and start a new game
    /// </summary>
    public void NewGame()
    {
        pole.Init();
        totalTime = 0;
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game });
        CurrentState = GameState.Playing;
    }

    /// <summary>
    /// End this game and display the results screen
    /// </summary>
    private void EndGame()
    {
        menuManager.SetEndTimerText(totalTime);
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.End });
        CurrentState = GameState.GameOver;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Pause the game
    /// </summary>
    public void Pause()
    {
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game, MenuCanvas.Pause });
        CurrentState = GameState.Paused;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Un-pause the game
    /// </summary>
    public void UnPause()
    {
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game });
        CurrentState = GameState.Playing;
    }

}
