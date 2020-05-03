using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainUI : MonoBehaviour, IMenuUIBase
{
    [SerializeField]
    SelectionGroup difficultySelection;

    private const float TWEEN_TIME = 0.3f;

    private RectTransform titlePanel;
    private RectTransform outlinePanel;
    private GameObject blocker;

    private bool active;

    void Awake()
    {
        outlinePanel = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        titlePanel = transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        blocker = transform.GetChild(2).gameObject;
        active = true;
    }

    public void Start()
    {
        //Select the saved difficulty on load
        if (difficultySelection.Selected.name != Config.Difficulty.ToString())
        {
            difficultySelection.defaultElement = (int)Config.Difficulty;
            difficultySelection.Select(difficultySelection.defaultElement);
        }
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

        LeanTween.moveX(outlinePanel, 0, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(titlePanel, 0, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);

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
        //Menu -> Game
        if(GameManager.Instance.MenuManager.CurrCanvases.Contains(MenuCanvas.Game))
        {
            LeanTween.moveY(outlinePanel, 102, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
            LeanTween.moveY(titlePanel, 102, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
        }
        //Menu -> Settings/Cosmetics
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
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }
}
