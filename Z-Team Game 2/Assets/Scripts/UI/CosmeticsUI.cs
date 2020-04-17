using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    Skin currentBackground;
    Skin currentPole;
    Skin currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        if(button.transform.parent.name == "BackgroundSelection")
        {
            currentSkinNum = (int)currentBackground;
        }
        else if (button.transform.parent.name == "PoleSelection")
        {
            currentSkinNum = (int)currentBackground;

        }
        else if (button.transform.parent.name == "EnemySelection")
        {
            currentSkinNum = (int)currentBackground;
        }

        int newSkin = currentSkinNum + dir;
        if (newSkin < 0)
            newSkin = Config.MaxSkins - 1;
        else if (newSkin >= Config.MaxSkins)
            newSkin = 0;
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

        }
        else if (button.transform.parent.name == "PoleSelection")
        {

        }
        else if (button.transform.parent.name == "EnemySelection")
        {

        }
    }
}
