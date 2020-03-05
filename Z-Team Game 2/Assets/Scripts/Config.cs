using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlScheme { Angle, Screen }

public class Config : Singleton<Config>
{
    public ControlScheme Scheme { get; private set; }
    public bool Invert { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    /// <summary>
    /// Set the control scheme based on boolean value
    /// </summary>
    /// <param name="val">True=screen, false=angle</param>
    public void SetControlScheme(bool val)
    {
        Scheme = val ? ControlScheme.Screen : ControlScheme.Angle;
    }

    /// <summary>
    /// Set the invert control option based on a boolean value
    /// </summary>
    /// <param name="val">True=on, false=off</param>
    public void SetInvertedControls(bool val)
    {
        Invert = val;
    }
}
