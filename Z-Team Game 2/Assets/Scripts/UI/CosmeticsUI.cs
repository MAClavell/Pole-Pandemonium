using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CosmeticsUI : MonoBehaviour, IMenuUIBase
{
    private const float TWEEN_TIME = 0.3f;

    [SerializeField]
    private SVGImage backgroundObj;
    [SerializeField]
    private SVGImage foregroundObj;
    [SerializeField]
    private SVGImage poleObj;
    [SerializeField]
    private SVGImage[] bounceEnemyObjs;
    [SerializeField]
    private SVGImage[] stickEnemyObjs;

    [SerializeField]
    private Button backgroundEquipButton;

    [SerializeField]
    private Button poleEquipButton;

    [SerializeField]
    private Button enemyEquipButton;

    private SkinType currentBackgroundSkin;
    private SkinType currentPoleSkin;
    private SkinType currentEnemySkin;

    private RectTransform titlePanel;
    private RectTransform outlinePanel;
    private RectTransform backgroundPanel;
    private RectTransform polePanel;
    private RectTransform enemyPanel;
    private RectTransform canvasRect;
    private RectTransform selectionSafeArea;
    private GameObject blocker;
    private bool active;

    void Awake()
    {
        active = true;
        outlinePanel = transform.GetChild(2).GetChild(0).GetComponent<RectTransform>();
        titlePanel = transform.GetChild(2).GetChild(1).GetComponent<RectTransform>();
        backgroundPanel = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        polePanel = transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
        enemyPanel = transform.GetChild(1).GetChild(2).GetComponent<RectTransform>();
        canvasRect = GetComponent<RectTransform>();
        selectionSafeArea = transform.GetChild(1).GetComponent<RectTransform>();
        blocker = transform.GetChild(3).gameObject;
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
        backgroundObj.gameObject.SetActive(true);
        poleObj.gameObject.SetActive(true);
        bounceEnemyObjs[0].gameObject.SetActive(true);
        stickEnemyObjs[0].gameObject.SetActive(true);

        //Set defaults after config file is loaded
        SetBackgroundSkinPreview(Config.BackgroundSkin);
        SetPoleSkinPreview(Config.PoleSkin);
        SetEnemySkinPreview(Config.EnemySkin);

        SetEquipButtonInteractable(backgroundEquipButton, false);
        SetEquipButtonInteractable(poleEquipButton, false);
        SetEquipButtonInteractable(enemyEquipButton, false);

        LeanTween.moveX(outlinePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(titlePanel, 0, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(backgroundPanel, 50, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(polePanel, 50, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(enemyPanel, 50, TWEEN_TIME).setEaseOutQuad().setIgnoreTimeScale(true);

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
        backgroundObj.gameObject.SetActive(false);
        poleObj.gameObject.SetActive(false);
        bounceEnemyObjs[0].gameObject.SetActive(false);
        stickEnemyObjs[0].gameObject.SetActive(false);

        LeanTween.moveX(outlinePanel, outlinePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(titlePanel, titlePanel.rect.width, TWEEN_TIME).setEaseInOutQuad().setIgnoreTimeScale(true);

        float safeAreaDiff = canvasRect.rect.xMin - selectionSafeArea.rect.xMin;
        LeanTween.moveX(backgroundPanel, -220 + safeAreaDiff, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(polePanel, -220 + safeAreaDiff, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);
        LeanTween.moveX(enemyPanel, -220 + safeAreaDiff, TWEEN_TIME).setEaseInQuad().setIgnoreTimeScale(true);

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
        LeanTween.cancel(enemyPanel);
        LeanTween.cancel(polePanel);
        LeanTween.cancel(backgroundPanel);
        LeanTween.cancel(outlinePanel);
        LeanTween.cancel(titlePanel);
        LeanTween.cancel(gameObject);
    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Cycle a skin
    /// </summary>
    public void OnCycleButtonClicked()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;

        int dir = 1;
        if(button.name == "LeftButton")
        {
            dir = -1;   
        }

        int currentSkinNum = 0;
        int skinType = 0; //1=background, 2=pole, 3=enemy
        if(button.transform.parent.parent.name == "BackgroundSelection")
        {
            currentSkinNum = (int)currentBackgroundSkin;
            skinType = 0;
        }
        else if (button.transform.parent.parent.name == "PoleSelection")
        {
            currentSkinNum = (int)currentPoleSkin;
            skinType = 1;
        }
        else if (button.transform.parent.parent.name == "EnemySelection")
        {
            currentSkinNum = (int)currentEnemySkin;
            skinType = 2;
        }

        //Calculate and keep in bounds
        int newVal = currentSkinNum + dir;
        if (newVal < 0)
            newVal = Config.MaxSkins - 1;
        else if (newVal >= Config.MaxSkins)
            newVal = 0;

        //Set preview displays
        SkinType newSkin = (SkinType)newVal;
        SkinType equippedSkin = SkinType.Default;
        Button equipButton = null;
        switch (skinType)
        {
            case 0: 
                SetBackgroundSkinPreview(newSkin);
                equippedSkin = Config.BackgroundSkin;
                equipButton = backgroundEquipButton;
                break;
            case 1: 
                SetPoleSkinPreview(newSkin);
                equippedSkin = Config.PoleSkin;
                equipButton = poleEquipButton;
                break;
            case 2: 
                SetEnemySkinPreview(newSkin);
                equippedSkin = Config.EnemySkin;
                equipButton = enemyEquipButton;
                break;
        }

        //Set equip button
        if(newSkin == equippedSkin)
        {
            SetEquipButtonInteractable(equipButton, false);
        }
        else SetEquipButtonInteractable(equipButton, true);
    }

    /// <summary>
    /// Set the preview display for the background
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetBackgroundSkinPreview(SkinType newSkin)
    {
        currentBackgroundSkin = newSkin;
        var skinObj = GameManager.Instance.Skins.GetSkin(newSkin);
        backgroundObj.sprite = skinObj.backgroundSprite;
        foregroundObj.sprite = skinObj.foregroundSprite;
        backgroundObj.color = skinObj.backgroundColor;
    }

    /// <summary>
    /// Set the preview display for the pole
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetPoleSkinPreview(SkinType newSkin)
    {
        currentPoleSkin = newSkin;
        var skinObj = GameManager.Instance.Skins.GetSkin(newSkin);
        poleObj.sprite = skinObj.poleSprite;
        poleObj.color = skinObj.poleColor;
    }

    /// <summary>
    /// Set the preview display for the enemy
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetEnemySkinPreview(SkinType newSkin)
    {
        currentEnemySkin = newSkin;
        var skinObj = GameManager.Instance.Skins.GetSkin(newSkin);

        //Set bounce enemy
        bounceEnemyObjs[0].sprite = skinObj.bounceEnemyTorsoSprite;
        bounceEnemyObjs[0].color = skinObj.bounceEnemyTorsoColor;
        bounceEnemyObjs[1].sprite = skinObj.bounceEnemyHeadSprite;
        bounceEnemyObjs[1].color = skinObj.bounceEnemyHeadColor;
        bounceEnemyObjs[2].sprite = skinObj.bounceEnemyArmSprite;
        bounceEnemyObjs[2].color = skinObj.bounceEnemyArmColor;
        bounceEnemyObjs[3].sprite = skinObj.bounceEnemyArmSprite;
        bounceEnemyObjs[3].color = skinObj.bounceEnemyArmColor;
        bounceEnemyObjs[4].sprite = skinObj.bounceEnemyLegSprite;
        bounceEnemyObjs[4].color = skinObj.bounceEnemyLegColor;
        bounceEnemyObjs[5].sprite = skinObj.bounceEnemyLegSprite;
        bounceEnemyObjs[5].color = skinObj.bounceEnemyLegColor;

        //Set stick enemy
        stickEnemyObjs[0].sprite = skinObj.stickEnemyTorsoSprite;
        stickEnemyObjs[0].color = skinObj.stickEnemyTorsoColor;
        stickEnemyObjs[1].sprite = skinObj.stickEnemyHeadSprite;
        stickEnemyObjs[1].color = skinObj.stickEnemyHeadColor;
        stickEnemyObjs[2].sprite = skinObj.stickEnemyArmSprite;
        stickEnemyObjs[2].color = skinObj.stickEnemyArmColor;
        stickEnemyObjs[3].sprite = skinObj.stickEnemyArmSprite;
        stickEnemyObjs[3].color = skinObj.stickEnemyArmColor;
        stickEnemyObjs[4].sprite = skinObj.stickEnemyLegSprite;
        stickEnemyObjs[4].color = skinObj.stickEnemyLegColor;
        stickEnemyObjs[5].sprite = skinObj.stickEnemyLegSprite;
        stickEnemyObjs[5].color = skinObj.stickEnemyLegColor;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Equip a skin
    /// </summary>
    public void OnEquipButtonClicked()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (button.transform.parent.parent.name == "BackgroundSelection")
        {
            Config.BackgroundSkin = currentBackgroundSkin;
            SetEquipButtonInteractable(backgroundEquipButton, false);
        }
        else if (button.transform.parent.parent.name == "PoleSelection")
        {
            Config.PoleSkin = currentPoleSkin;
            SetEquipButtonInteractable(poleEquipButton, false);
        }
        else if (button.transform.parent.parent.name == "EnemySelection")
        {
            Config.EnemySkin = currentEnemySkin;
            SetEquipButtonInteractable(enemyEquipButton, false);
        }
    }

    /// <summary>
    /// Set an equip button to an interactable or not interactable state
    /// </summary>
    /// <param name="b">The button</param>
    /// <param name="interactable">Interactable or not</param>
    private void SetEquipButtonInteractable(Button b, bool interactable)
    {
        b.interactable = interactable;
        if(interactable)
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equip";
        else b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equipped";
    }
}
