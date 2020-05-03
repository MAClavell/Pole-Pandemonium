using Crystal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum GameState { Loading, MainMenu, Playing, Paused, GameOver }

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

    public MenuManager MenuManager { get => menuManager; }

    public EnemyManager EnemyManager { get; private set; }

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer foreground;
    [SerializeField] private GameObject movingSprites;
    [SerializeField] private Pole pole;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private SkinScriptableObject skins;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioClip easy;
    [SerializeField] private AudioClip medium;
    [SerializeField] private AudioClip hard;

    public double GameTime { get; private set; }

    private AudioSource gameOverSound;

    private Transform movingSpritesParent;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentState = GameState.MainMenu;
        gameOverSound = GameObject.Find("gameOverSound").GetComponent<AudioSource>();
        EnemyManager = GetComponent<EnemyManager>();
        Config.Load();
        Leaderboard.Init();
        movingSpritesParent = GameObject.Find("MovingSprites").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Config.SetInitialValues();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case GameState.Loading:
                CurrentState = GameState.MainMenu;
                break;

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
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Pause, MenuCanvas.Game });
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
    public void UnPause()
    {
        Physics.autoSimulation = true;
        menuManager.SetActiveCanvases(new MenuCanvas[] { MenuCanvas.Game });
        CurrentState = GameState.Playing;
        Time.timeScale = 1.0f;
        
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Set the difficulty of the next game
    /// </summary>
    public void SetDifficulty(Button prev, Button curr)
    {
        //Easy
        if(curr.name == "Easy")
        {
            Config.Difficulty = Difficulty.Easy;
            BGM.Stop();
            BGM.clip = easy;
            BGM.Play();
        }
        //Medium
        else if (curr.name == "Medium")
        {
            Config.Difficulty = Difficulty.Medium;
            BGM.Stop();
            BGM.clip = medium;
            BGM.Play();
        }
        //Hard
        else if (curr.name == "Hard")
        {
            Config.Difficulty = Difficulty.Hard;
            BGM.Stop();
            BGM.clip = hard;
            BGM.Play();
        }
    }

    public void SetMuteMusic(bool muted)
    {
        if(muted)
            audioMixer.SetFloat("MusicVolume", -80);
        else
            audioMixer.SetFloat("MusicVolume", 0);
    }

    public void SetMuteSoundEffects(bool muted)
    {
        if(muted)
            audioMixer.SetFloat("SoundEffectVolume", -80);
        else
            audioMixer.SetFloat("SoundEffectVolume", 0);
    }
}
