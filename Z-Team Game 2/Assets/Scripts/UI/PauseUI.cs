using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseUI : MonoBehaviour, IMenuUIBase
{
    private const float TWEEN_TIME = 0.3f;

    private RectTransform titlePanel;
    private RectTransform outlinePanel;
    private GameObject blocker;

    private bool active;
    private bool first;

    void Awake()
    {
        active = true;
        outlinePanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        titlePanel = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        blocker = transform.GetChild(1).gameObject;
        first = true;
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

        //Two types of enter animations
        //Game -> Pause
        if (GameManager.Instance.MenuManager.PrevCanvases.Contains(MenuCanvas.Game))
        {
            LeanTween.moveY(outlinePanel, 0, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
            LeanTween.moveY(titlePanel, 0, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
            GameManager.Instance.MenuManager.SetFilterColor(MenuManager.DEFAULT_FILTER_IN_COLOR, TWEEN_TIME);
        }
        //Settings/Cosmetics -> Pause
        else
        {
            LeanTween.moveX(outlinePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
            LeanTween.moveX(titlePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        }

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

        //Two types of exit animations
        //Pause -> Game
        if (GameManager.Instance.MenuManager.CurrCanvases.Contains(MenuCanvas.Game) || first)
        {
            first = false;
            LeanTween.moveY(outlinePanel, 102, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
            LeanTween.moveY(titlePanel, 102, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
            GameManager.Instance.MenuManager.SetFilterColor(MenuManager.DEFAULT_FILTER_OUT_COLOR, TWEEN_TIME);
        }
        //Pause -> Settings/Cosmetics
        else
        {
            LeanTween.moveX(outlinePanel, -outlinePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
            LeanTween.moveX(titlePanel, -titlePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        }

        LeanTween.value(gameObject, 0, 1, TWEEN_TIME + 0.05f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            blocker.SetActive(false);
            gameObject.SetActive(false);
        });
    }
    
    private void CancelTweens()
    {
        LeanTween.cancel(outlinePanel);
        LeanTween.cancel(titlePanel);
        LeanTween.cancel(gameObject);
    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }
}
