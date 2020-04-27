using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/SkinScriptableObject", order = 1)]
public class SkinScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Skin
    {
        public SkinType skinType;

        [Header("Background")]
        public Sprite backgroundSprite;
        public Sprite foregroundSprite;
        public GameObject movingObjectPrefab;
        public Color backgroundColor;

        [Header("Pole")]
        public Sprite poleSprite;
        public Color poleColor;

        
        [Header("Bounce Enemies")]
        public Sprite bounceEnemyHeadSprite;
        public Color bounceEnemyHeadColor;
        public Sprite bounceEnemyTorsoSprite;
        public Color bounceEnemyTorsoColor;
        public Sprite bounceEnemyArmSprite;
        public Color bounceEnemyArmColor;
        public Sprite bounceEnemyLegSprite;
        public Color bounceEnemyLegColor;

        [Header("Stick Enemies")]
        public Sprite stickEnemyHeadSprite;
        public Color stickEnemyHeadColor;
        public Sprite stickEnemyTorsoSprite;
        public Color stickEnemyTorsoColor;
        public Sprite stickEnemyArmSprite;
        public Color stickEnemyArmColor;
        public Sprite stickEnemyLegSprite;
        public Color stickEnemyLegColor;
    }

    public Skin[] skins;

    /// <summary>
    /// Get the correct skin data for a skin type
    /// </summary>
    /// <param name="skinType">Type to get</param>
    /// <returns>Returns a skin data object or null</returns>
    public Skin GetSkin(SkinType skinType)
    {
        return skins.FirstOrDefault(s => s.skinType == skinType);
    }
}
