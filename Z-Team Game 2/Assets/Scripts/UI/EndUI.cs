using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUI : MonoBehaviour, IMenuUIBase
{
    [SerializeField]
    SelectionGroup difficultySelection;

    private bool active;

    void Awake()
    {
        active = true;
    }

    /// <summary>
    /// Activate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Activate()
    {
        active = true;
        gameObject.SetActive(true);
        difficultySelection.defaultElement = (int)Config.Difficulty;
        difficultySelection.SelectNoInvoke(difficultySelection.defaultElement);
    }

    /// <summary>
    /// Deactivate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);

    }

    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    public GameObject GameObject { get => gameObject; }

    public bool Active { get => active; }
}
