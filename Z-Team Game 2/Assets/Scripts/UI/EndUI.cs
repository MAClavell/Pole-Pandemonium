using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUI : MonoBehaviour, IMenuUIBase
{
    [SerializeField]
    SelectionGroup difficultySelection;

    /// <summary>
    /// Activate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Activate(bool previouslyActive)
    {
        difficultySelection.defaultElement = (int)Config.Difficulty;
        difficultySelection.SelectNoInvoke(difficultySelection.defaultElement);
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
    public GameObject GameObject { get => gameObject; }
}
