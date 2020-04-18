using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CosmeticsUI : MonoBehaviour
{
    [SerializeField]
    SVGImage backgroundObj;

    [SerializeField]
    SVGImage poleObj;

    [SerializeField]
    SVGImage[] hitEnemyObjs;

    [SerializeField]
    SVGImage[] stickEnemyObjs;

    SkinType currentBackgroundSkin;
    SkinType currentPoleSkin;
    SkinType currentEnemySkin;

    // Start is called before the first frame update
    void Start()
    {
        //Set defaults after config file is loaded
        SetBackgroundSkinPreview(Config.BackgroundSkin);
        SetPoleSkinPreview(Config.PoleSkin);
        SetEnemySkinPreview(Config.EnemySkin);
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Cycle a skin
    /// </summary>
    void OnCycleButtonClicked()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;

        int dir = 1;
        if(button.name == "LeftButton")
        {
            dir = -1;   
        }

        int currentSkinNum = 0;
        int skinType = 0; //1=background, 2=pole, 3=enemy
        if(button.transform.parent.name == "BackgroundSelection")
        {
            currentSkinNum = (int)currentBackgroundSkin;
            skinType = 0;
        }
        else if (button.transform.parent.name == "PoleSelection")
        {
            currentSkinNum = (int)currentBackgroundSkin;
            skinType = 1;
        }
        else if (button.transform.parent.name == "EnemySelection")
        {
            currentSkinNum = (int)currentBackgroundSkin;
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
        switch(skinType)
        {
            case 0: SetBackgroundSkinPreview(newSkin); break;
            case 1: SetPoleSkinPreview(newSkin); break;
            case 2: SetEnemySkinPreview(newSkin); break;
        }
    }

    /// <summary>
    /// Set the preview display for the background
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetBackgroundSkinPreview(SkinType newSkin)
    {
        currentBackgroundSkin = newSkin;
    }

    /// <summary>
    /// Set the preview display for the pole
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetPoleSkinPreview(SkinType newSkin)
    {
        currentPoleSkin = newSkin;
    }

    /// <summary>
    /// Set the preview display for the enemy
    /// </summary>
    /// <param name="newSkin">Skin to set</param>
    private void SetEnemySkinPreview(SkinType newSkin)
    {
        currentEnemySkin = newSkin;
    }

    /// <summary>
    /// [UI EVENT CALLBACK]
    /// Equip a skin
    /// </summary>
    void OnEquipButtonClicked()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (button.transform.parent.name == "BackgroundSelection")
        {
            Config.BackgroundSkin = currentBackgroundSkin;
        }
        else if (button.transform.parent.name == "PoleSelection")
        {
            Config.PoleSkin = currentPoleSkin;
        }
        else if (button.transform.parent.name == "EnemySelection")
        {
            Config.EnemySkin = currentEnemySkin;
        }

        SetEquipButtonInteractable(button.transform.parent.Find("EquipButton").GetComponent<Button>(), false);
    }

    /// <summary>
    /// Set an equip button to an interactable or not interactable state
    /// </summary>
    /// <param name="b">The button</param>
    /// <param name="interactable">Interactable or not</param>
    void SetEquipButtonInteractable(Button b, bool interactable)
    {
        b.interactable = interactable;
        if(interactable)
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equip";
        else b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equipped";
    }
}
