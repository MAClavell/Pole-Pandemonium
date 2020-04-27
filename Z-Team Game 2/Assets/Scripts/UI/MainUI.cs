using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour, IMenuUIBase
{
    [SerializeField]
    SelectionGroup difficultySelection;

    /// <summary>
    /// Activate the UI
    /// </summary>
    public void Activate(bool previouslyActive)
    {
        difficultySelection.defaultElement = (int)Config.Difficulty;
        difficultySelection.Select(difficultySelection.defaultElement);
    }

    /// <summary>
    /// Deactivate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Deactivate(bool previouslyActive)
    {

    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public GameObject GameObject { get => gameObject; }
}
