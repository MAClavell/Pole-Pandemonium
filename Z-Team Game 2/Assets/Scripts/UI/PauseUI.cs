using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour, IMenuUIBase
{
    /// <summary>
    /// Activate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    public void Activate(bool previouslyActive)
    {

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
