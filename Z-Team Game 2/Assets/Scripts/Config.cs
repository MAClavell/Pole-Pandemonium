using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public enum ControlScheme { Angle, Screen }

public enum Difficulty { Easy=0, Medium, Hard}

public enum SkinType { Default=0, Demon, Rainbow, Panda }

public class Config
{
    /// <summary>
    /// Internal class for serializing the config
    /// </summary>
    [System.Serializable]
    private class SerializableConfig
    {
        public string versionNumber = "";
        public ControlScheme scheme = ControlScheme.Angle;
        public bool invert = false;
        public SkinType backgroundSkin = SkinType.Default;
        public SkinType poleSkin = SkinType.Default;
        public SkinType enemySkin = SkinType.Default;
        public Difficulty difficulty = Difficulty.Medium;
        public bool muteMusic = false;
        public bool muteSoundEffects = false;
    }

    /// <summary>
    /// Saved control scheme
    /// </summary>
    public static ControlScheme ControlScheme 
    {
        get => configFile.scheme; 
        set
        {
            configFile.scheme = value;
            if(!loading)
                SaveConfig();
        }
    }

    /// <summary>
    /// Saved invert status
    /// </summary>
    public static bool Invert 
    {
        get => configFile.invert;
        set
        {
            configFile.invert = value;
            if(!loading)
                SaveConfig();
        }
    }

    /// <summary>
    /// Saved background skin
    /// </summary>
    public static SkinType BackgroundSkin
    {
        get => configFile.backgroundSkin;
        set
        {
            configFile.backgroundSkin = value;
            if (!loading)
                SaveConfig();

            //Set the actual backgrounds's sprite
            var gm = GameManager.Instance;
            var skin = gm.Skins.GetSkin(value);
            gm.Background.sprite = skin.backgroundSprite;
            gm.Foreground.sprite = skin.foregroundSprite ? skin.foregroundSprite : null;
            gm.SetMovingSprites(skin.movingObjectPrefab ? skin.movingObjectPrefab : null);
            gm.Background.color = skin.backgroundColor;
        }
    }

    /// <summary>
    /// Saved pole skin
    /// </summary>
    public static SkinType PoleSkin 
    { 
        get => configFile.poleSkin;
        set
        {
            configFile.poleSkin = value;
            if(!loading)
                SaveConfig();

            //Set the actual pole's sprite
            var gm = GameManager.Instance;
            var skin = gm.Skins.GetSkin(value);
            gm.Pole.SpriteRenderer.sprite = skin.poleSprite;
            gm.Pole.SpriteRenderer.color = skin.poleColor;
        }
    }
    
    /// <summary>
    /// Saved enemy skin
    /// </summary>
    public static SkinType EnemySkin 
    { 
        get => configFile.enemySkin;
        set
        {
            configFile.enemySkin = value;
            if(!loading)
                SaveConfig();

            var gm = GameManager.Instance;
            var skin = gm.Skins.GetSkin(value);

            //Set the actual bounce enemy's sprites
            var bounce = gm.EnemyManager.BounceEnemyPrefab.transform;
            var bounceTorso = bounce.GetChild(0).GetComponent<SpriteRenderer>();
            bounceTorso.sprite = skin.bounceEnemyTorsoSprite;
            bounceTorso.color = skin.bounceEnemyTorsoColor;
            var bounceHead = bounce.GetChild(1).GetComponent<SpriteRenderer>();
            bounceHead.sprite = skin.bounceEnemyHeadSprite;
            bounceHead.color = skin.bounceEnemyHeadColor;
            var bounceLArm = bounce.GetChild(2).GetComponent<SpriteRenderer>();
            bounceLArm.sprite = skin.bounceEnemyArmSprite;
            bounceLArm.color = skin.bounceEnemyArmColor;
            var bounceRArm = bounce.GetChild(3).GetComponent<SpriteRenderer>();
            bounceRArm.sprite = skin.bounceEnemyArmSprite;
            bounceRArm.color = skin.bounceEnemyArmColor;
            var bounceLLeg = bounce.GetChild(4).GetComponent<SpriteRenderer>();
            bounceLLeg.sprite = skin.bounceEnemyLegSprite;
            bounceLLeg.color = skin.bounceEnemyLegColor;
            var bounceRLeg = bounce.GetChild(5).GetComponent<SpriteRenderer>();
            bounceRLeg.sprite = skin.bounceEnemyLegSprite;
            bounceRLeg.color = skin.bounceEnemyLegColor;

            //Set the actual stick enemy's sprites
            var stick = gm.EnemyManager.StickEnemyPrefab.transform;
            var stickTorso = stick.GetChild(0).GetComponent<SpriteRenderer>();
            stickTorso.sprite = skin.stickEnemyTorsoSprite;
            stickTorso.color = skin.stickEnemyTorsoColor;
            var stickHead = stick.GetChild(1).GetComponent<SpriteRenderer>();
            stickHead.sprite = skin.stickEnemyHeadSprite;
            stickHead.color = skin.stickEnemyHeadColor;
            var stickLArm = stick.GetChild(2).GetComponent<SpriteRenderer>();
            stickLArm.sprite = skin.stickEnemyArmSprite;
            stickLArm.color = skin.stickEnemyArmColor;
            var stickRArm = stick.GetChild(3).GetComponent<SpriteRenderer>();
            stickRArm.sprite = skin.stickEnemyArmSprite;
            stickRArm.color = skin.stickEnemyArmColor;
            var stickLLeg = stick.GetChild(4).GetComponent<SpriteRenderer>();
            stickLLeg.sprite = skin.stickEnemyLegSprite;
            stickLLeg.color = skin.stickEnemyLegColor;
            var stickRLeg = stick.GetChild(5).GetComponent<SpriteRenderer>();
            stickRLeg.sprite = skin.stickEnemyLegSprite;
            stickRLeg.color = skin.stickEnemyLegColor;
        }
    }

	/// <summary>
	/// The max amount of skin types
	/// </summary>
	public static int MaxSkins { get; } = System.Enum.GetValues(typeof(SkinType)).Length;

    /// <summary>
    /// The current difficulty of the game
    /// </summary>
    public static Difficulty Difficulty 
    {
        get => configFile.difficulty;
        set
        {
            configFile.difficulty = value;
            if (!loading)
                SaveConfig();

            GameManager.Instance.EnemyManager.SetDifficulty(value);
        }
    }

    /// <summary>
    /// Whether music is muted
    /// </summary>
    public static bool MuteMusic
    {
        get => configFile.muteMusic;
        set
        {
            configFile.muteMusic = value;
            if (!loading)
                SaveConfig();
        }
    }

    /// <summary>
    /// Whether sound effects are muted
    /// </summary>
    public static bool MuteSoundEffects
    {
        get => configFile.muteSoundEffects;
        set
        {
            configFile.muteSoundEffects = value;
            if (!loading)
                SaveConfig();
        }
    }

    private const string CONFIG_PATH = "/Config.pole";
    private const string VERSION_NUMBER = "1.0";
    private static SerializableConfig configFile;
    private static bool loading = true;

    public static void Load()
    {
        //If there is an error loading, delete the file and load defaults
        try
        {
            LoadConfigFile();
        }
        catch when (File.Exists(Application.persistentDataPath + CONFIG_PATH))
        {
            File.Delete(Application.persistentDataPath + CONFIG_PATH);
            LoadConfigFile();
        }
    }

    /// <summary>
    /// Load the config file from disc
    /// ONLY RUN ONCE AT STARTUP
    /// </summary>
    private static void LoadConfigFile()
    {
        //Only load if the config file exists
        if (!File.Exists(Application.persistentDataPath + CONFIG_PATH) ||
            string.IsNullOrWhiteSpace(Application.persistentDataPath + CONFIG_PATH))
        {
            configFile = new SerializableConfig();
            SaveConfig();
            return;
        }

        //Load controls from config file
        using (StreamReader sr = new StreamReader(Application.persistentDataPath + CONFIG_PATH))
        {
            configFile = JsonUtility.FromJson<SerializableConfig>(sr.ReadToEnd());
        }
    }
    /// <summary>
    /// Set the initial values of the config
    /// ONLY RUN ONCE AT STARTUP
    /// </summary>
    public static void SetInitialValues()
    {
        loading = true;
        BackgroundSkin = BackgroundSkin;
        PoleSkin = PoleSkin;
        EnemySkin = EnemySkin;
        Difficulty = Difficulty;
        loading = false;
    }
    /// <summary>
    /// Save the current game time as the new highscore
    /// </summary>
    private static void SaveConfig()
    {
        //Ensure the version numbers are consistent
        if (configFile.versionNumber != VERSION_NUMBER)
        {
            configFile.versionNumber = VERSION_NUMBER;
        }

        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + CONFIG_PATH, false))
        {
            sw.Write(JsonUtility.ToJson(configFile));
        }
    }
}
