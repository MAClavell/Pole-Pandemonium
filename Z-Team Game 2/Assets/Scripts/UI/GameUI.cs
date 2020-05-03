using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour, IMenuUIBase
{
    private const float TWEEN_TIME = 0.3f;

    private RectTransform timerPanel;
    private RectTransform pausePanel;
    private GameObject blocker;

    private bool active;

    void Awake()
    {
        timerPanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        pausePanel = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
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

    private void CancelTweens()
    {
        LeanTween.cancel(timerPanel);
        LeanTween.cancel(pausePanel);
        LeanTween.cancel(gameObject);
    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }
}
