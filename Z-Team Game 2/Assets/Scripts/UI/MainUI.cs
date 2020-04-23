using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    SelectionGroup difficultySelection;

    // Start is called before the first frame update
    void Start()
    {
        difficultySelection.defaultElement = (int)Config.Difficulty;
        difficultySelection.SelectNoInvoke(difficultySelection.defaultElement);
    }
}
