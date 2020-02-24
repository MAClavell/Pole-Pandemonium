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

    // Start is called before the first frame update
    void Awake()
    {
        CurrentState = GameState.MainMenu;
        Resize();
    }

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    /// <summary>
    /// Resize the game based on the height of the device
    /// </summary>
    private void Resize()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        float aspectRatio = resolution.x / resolution.y;

       // Camera.main.aspect = aspectRatio;
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

                if (Mathf.Abs(pole.Rotation) > LOSING_ROTATION)
                    EndGame();
                break;
            
            case GameState.Paused:
                break;
            
            case GameState.GameOver:
                NewGame();
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
        CurrentState = GameState.Playing;
    }

    /// <summary>
    /// End this game and display the results screen
    /// </summary>
    private void EndGame()
    {
        CurrentState = GameState.GameOver;
    }

    /// <summary>
    /// Pause the game
    /// Used by UI events
    /// </summary>
    public void Pause()
    {
        CurrentState = GameState.Paused;
    }

    /// <summary>
    /// Un-pause the game
    /// Used by UI events
    /// </summary>
    public void UnPause()
    {
        CurrentState = GameState.Playing;
    }

}
