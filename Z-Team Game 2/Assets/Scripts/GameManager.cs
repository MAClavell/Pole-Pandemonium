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

    /// <summary>
    /// Get the pole object
    /// </summary>
    public Pole Pole { get => pole; }

    /// <summary>
    /// Get the background sprite object
    /// </summary>
    public SpriteRenderer Background { get => background; }

    /// <summary>
    /// Get the movingSprites game object
    /// </summary>
    public GameObject MovingSprites { get => movingSprites; }

    /// <summary>
    /// Get the foreground sprite object
    /// </summary>
    public SpriteRenderer Foreground { get => foreground; }

    /// <summary>
    /// Get the scriptable object containing all the skins
    /// </summary>
    public SkinScriptableObject Skins { get => skins; }

    public EnemyManager EnemyManager { get; private set; }

    [SerializeField]
    private SpriteRenderer background;
    [SerializeField]
    private SpriteRenderer foreground;
    [SerializeField]
    private GameObject movingSprites;
    [SerializeField]
    private Pole pole;
    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    SkinScriptableObject skins;

    public double GameTime { get; private set; }

    private AudioSource gameOverSound;

    private Transform movingSpritesParent;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentState = GameState.MainMenu;
        gameOverSound = GameObject.Find("gameOverSound").GetComponent<AudioSource>();
        EnemyManager = GetComponent<EnemyManager>();
        movingSpritesParent = GameObject.Find("MovingSprites").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Config.Init();
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
                GameTime += Time.deltaTime;
                menuManager.SetTimerText(GameTime);

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
        GameTime = 0;
        EnemyManager.SetDifficulty();
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game });
        CurrentState = GameState.Playing;
        Time.timeScale = 1.0f;

        //Used to find if there are any enemies on the screen. Before this was here, if the playr restarted and there was still enemies on screen, they would persist to the next round
        foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(item);
        }
        
    }

    /// <summary>
    /// End this game and display the results screen
    /// </summary>
    private void EndGame()
    {
        menuManager.SetEndTimerText(GameTime);
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.End });
        CurrentState = GameState.GameOver;
        gameOverSound.Play();
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Pause the game
    /// </summary>
    public void Pause()
    {
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game, MenuCanvas.Pause });
        Physics.autoSimulation = false;
        CurrentState = GameState.Paused;
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Sets the current moving sprites game object and instantiates it in the scene
    /// </summary>
    public void SetMovingSprites(GameObject movingSprites)
    {
        // Remove the current moving game object
        int currentChildCount = movingSpritesParent.childCount;

        foreach (Transform child in movingSpritesParent)
        {
            Destroy(child.gameObject);
        }

        if (!movingSprites) return;

        GameObject newObj = Instantiate(movingSprites, movingSpritesParent, true);

        newObj.SetActive(true);
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Un-pause the game
    /// </summary>
    /// </summary>
    public void UnPause()
    {
        Physics.autoSimulation = true;
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game });
        CurrentState = GameState.Playing;
        Time.timeScale = 1.0f;
        
    }

}
