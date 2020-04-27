using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour, IMenuUIBase
{
    [SerializeField] Toggle controlToggle;
    [SerializeField] Toggle invertToggle;
    [SerializeField] Toggle muteMusicToggle;
    [SerializeField] Toggle muteSoundEffectsToggle;


    /// <summary>
    /// Activate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Activate(bool previouslyActive)
    {
        controlToggle.isOn = Config.ControlScheme == ControlScheme.Screen ? true : false;
        invertToggle.isOn = Config.Invert;
        muteMusicToggle.isOn = Config.MuteMusic;
        muteSoundEffectsToggle.isOn = Config.MuteSoundEffects;
    }

    /// <summary>
    /// Deactivate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Deactivate(bool previouslyActive)
    {

    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for when the control scheme option is changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnControlToggleChanged(Toggle tog)
    {
        Config.ControlScheme = tog.isOn ? ControlScheme.Screen : ControlScheme.Angle;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for when the inverted controls option is changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnInvertToggleChanged(Toggle tog)
    {
        Config.Invert = tog.isOn;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for when the mute music option is changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnMuteMusicToggleChanged(Toggle tog)
    {
        Config.MuteMusic = tog.isOn;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// The toggle for when the mute sound effects option is changed
    /// </summary>
    /// <param name="tog">Toggle object</param>
    public void OnMuteSoundEffectsToggleChanged(Toggle tog)
    {
        Config.MuteSoundEffects = tog.isOn;
    }
}
