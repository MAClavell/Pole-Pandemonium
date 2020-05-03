using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour, IMenuUIBase
{
    private const float TWEEN_TIME = 0.3f;
    
    private RectTransform titlePanel;
    private RectTransform outlinePanel;
    private RectTransform settingsPanel;
    private CanvasGroup creditsPanel;
    private GameObject blocker;

    private Toggle controlToggle;
    private Toggle invertToggle;
    private Toggle muteMusicToggle;
    private Toggle muteSoundEffectsToggle;

    private bool active;

    void Awake()
    {
        active = true;
        outlinePanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        titlePanel = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        settingsPanel = transform.GetChild(0).GetChild(2).GetComponent<RectTransform>();
        creditsPanel = titlePanel.GetChild(0).GetComponent<CanvasGroup>();
        blocker = transform.GetChild(1).gameObject;

        //Get toggles
        controlToggle = settingsPanel.GetChild(0).GetChild(3).GetChild(0).GetComponent<Toggle>();
        invertToggle = settingsPanel.GetChild(1).GetChild(3).GetChild(0).GetComponent<Toggle>();
        muteMusicToggle = settingsPanel.GetChild(2).GetChild(3).GetChild(0).GetComponent<Toggle>();
        muteSoundEffectsToggle = settingsPanel.GetChild(3).GetChild(3).GetChild(0).GetComponent<Toggle>();
    }

    /// <summary>
    /// Activate the UI
    /// </summary>
    public void Activate()
    {
        CancelTweens();
        active = true;
        gameObject.SetActive(true);
        blocker.SetActive(true);
        controlToggle.isOn = Config.ControlScheme == ControlScheme.Screen ? true : false;
        invertToggle.isOn = Config.Invert;
        muteMusicToggle.isOn = Config.MuteMusic;
        muteSoundEffectsToggle.isOn = Config.MuteSoundEffects;

        LeanTween.moveX(outlinePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(titlePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(settingsPanel, -250.6f, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(creditsPanel, 1f, TWEEN_TIME / 2).setEaseInOutQuad().setIgnoreTimeScale(true);

        //Only tween out filter panel if we are coming from the main menu
        if (GameManager.Instance.MenuManager.PrevCanvases.Contains(MenuCanvas.Main))
            GameManager.Instance.MenuManager.SetFilterColor(MenuManager.DEFAULT_FILTER_IN_COLOR, TWEEN_TIME);

        LeanTween.value(gameObject, 0f, 1f, TWEEN_TIME + 0.05f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            blocker.SetActive(false);
        });
    }

    /// <summary>
    /// Deactivate the UI
    /// </summary>
    public void Deactivate()
    {
        CancelTweens();
        active = false;
        blocker.SetActive(true);

        LeanTween.moveX(outlinePanel, outlinePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(titlePanel, titlePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(settingsPanel, -settingsPanel.rect.width*2, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(creditsPanel, 0f, TWEEN_TIME / 2).setEaseInOutQuad().setIgnoreTimeScale(true);

        //Only tween out filter panel if we are going back to the main menu
        if (GameManager.Instance.MenuManager.CurrCanvases.Contains(MenuCanvas.Main))
            GameManager.Instance.MenuManager.SetFilterColor(MenuManager.DEFAULT_FILTER_OUT_COLOR, TWEEN_TIME);

        LeanTween.value(gameObject, 0, 1, TWEEN_TIME + 0.05f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            blocker.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    private void CancelTweens()
    {
        LeanTween.cancel(creditsPanel.gameObject);
        LeanTween.cancel(settingsPanel);
        LeanTween.cancel(outlinePanel);
        LeanTween.cancel(titlePanel);
        LeanTween.cancel(gameObject);
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

    public bool Active { get => active; }
}
