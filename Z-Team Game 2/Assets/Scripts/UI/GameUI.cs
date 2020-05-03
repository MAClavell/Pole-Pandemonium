using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour, IMenuUIBase
{
    private const float TWEEN_TIME = 0.3f;
    private const float GRAVITY_TWEEN_TIME = 0.3f;

    private RectTransform timerPanel;
    private RectTransform pausePanel;
    private RectTransform gravityTextPanel;
    private TextMeshProUGUI gravityScaleText;
    private TextMeshProUGUI gravityText;
    private GameObject blocker;

    private bool active;

    void Awake()
    {
        timerPanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        pausePanel = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        gravityTextPanel = timerPanel.GetChild(3).GetComponent<RectTransform>();
        gravityScaleText = gravityTextPanel.GetComponent<TextMeshProUGUI>();
        gravityText = gravityScaleText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        blocker = transform.GetChild(2).gameObject;
        active = true;
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

        LeanTween.moveX(timerPanel, 0, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(pausePanel, 5, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);

        LeanTween.value(gameObject, 0, 1, TWEEN_TIME + 0.05f).setIgnoreTimeScale(true).setOnComplete(() =>
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

        LeanTween.moveX(timerPanel, 201, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(pausePanel, -35, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);

        LeanTween.value(gameObject, 0, 1, TWEEN_TIME + 0.05f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            blocker.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    public void UpdateGravityText(float gravityScale)
    {
        CancelGravityTextTween();
        gravityScaleText.text = $"x {gravityScale.ToString("n1")}";

        LeanTween.scale(gravityTextPanel, new Vector3(2f, 2f, 2f), GRAVITY_TWEEN_TIME).setEaseInOutQuad();
        LeanTween.value(gravityTextPanel.gameObject, (float val) => gravityText.alpha = val, 0f, 1f, GRAVITY_TWEEN_TIME);
        LeanTween.moveY(gravityTextPanel, -45, GRAVITY_TWEEN_TIME).setEaseInOutQuad().setOnComplete(() =>
        {
            LeanTween.scale(gravityTextPanel, new Vector3(1, 1, 1), GRAVITY_TWEEN_TIME).setEaseInOutQuad().setDelay(1f);
            LeanTween.value(gravityTextPanel.gameObject, (float val) => gravityText.alpha = val, 1f, 0f, GRAVITY_TWEEN_TIME).setDelay(1f);
            LeanTween.moveY(gravityTextPanel, 0, GRAVITY_TWEEN_TIME).setEaseInOutQuad().setDelay(1f);
        });
    }

    private void CancelTweens()
    {
        LeanTween.cancel(timerPanel);
        LeanTween.cancel(pausePanel);
        LeanTween.cancel(gameObject);
    }

    private void CancelGravityTextTween()
    {
        LeanTween.cancel(gravityTextPanel);
    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }
}
