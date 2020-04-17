using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ControlScheme { Angle, Screen }

public enum Skin { Default=0, Demon }

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
        public Skin poleSkin = Skin.Default;
        public Skin enemySkin = Skin.Default;
        public Skin backgroundSkin = Skin.Default;
    }


    public static ControlScheme Scheme { get => configFile.scheme; }
    public static bool Invert { get => configFile.invert;}
    public static Skin PoleSkin { get => configFile.poleSkin; }
    public static Skin EnemySkin { get => configFile.enemySkin; }
    public static Skin BackgroundSkin { get => configFile.backgroundSkin; }
    public static short MaxSkins { get => 2; }

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

    /// <summary>
    /// Set the control scheme based on boolean value
    /// </summary>
    /// <param name="val">True=screen, false=angle</param>
    public static void SetControlScheme(bool val)
    {
        configFile.scheme = val ? ControlScheme.Screen : ControlScheme.Angle;
        SaveConfig();
    }

    /// <summary>
    /// Set the invert control option based on a boolean value
    /// </summary>
    /// <param name="val">True=on, false=off</param>
    public static void SetInvertedControls(bool val)
    {
        configFile.invert = val;
        SaveConfig();
    }
}
