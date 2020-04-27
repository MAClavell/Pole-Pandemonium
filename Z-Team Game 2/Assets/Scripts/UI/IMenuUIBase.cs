using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for controlling UI activation and deactivation behaviours
/// </summary>
public interface IMenuUIBase
{
    /// <summary>
    /// Get the gameobject attached to this UI
    /// </summary>
    GameObject GameObject { get; }

    /// <summary>
    /// Activate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    void Activate(bool previouslyActive);

    /// <summary>
    /// Deactivate the UI
    /// </summary>
    /// <param name="previouslyActive">Whether the UI is currently active</param>
    void Deactivate(bool previouslyActive);
}
