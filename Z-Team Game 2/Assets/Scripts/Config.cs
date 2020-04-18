using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ControlScheme { Angle, Screen }

public enum SkinType { Default=0, Demon }

public class Config
{
    /// <summary>
    /// Internal class for serializing the config
    /// </summary>
    [System.Serializable]
    private class SerializableConfig
    {
        public ControlScheme scheme = ControlScheme.Angle;
        public bool invert = false;
        public SkinType poleSkin = SkinType.Default;
        public SkinType enemySkin = SkinType.Default;
        public SkinType backgroundSkin = SkinType.Default;
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
            SaveConfig();
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
            configFile.backgroundSkin = value;
            SaveConfig();
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
            configFile.poleSkin = value;
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
            configFile.enemySkin = value;
            SaveConfig();
        }
    }

    /// <summary>
    /// The max amount of skin types
    /// </summary>
    public static int MaxSkins { get => System.Enum.GetValues(typeof(SkinType)).Length; }

    private static SerializableConfig configFile;
    private static string CONFIG_PATH = "/Config.pole";

    /// <summary>
    /// Init the config file
    /// ONLY RUN ONCE AT STARTUP
    /// </summary>
    public static void Init()
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
    /// Save the current game time as the new highscore
    /// </summary>
    private static void SaveConfig()
    {
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + CONFIG_PATH, false))
        {
            string json = JsonUtility.ToJson(configFile);
            sw.Write(JsonUtility.ToJson(configFile));
        }
    }
}
