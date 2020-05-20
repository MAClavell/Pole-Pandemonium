using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitToScreen : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        FitToHeight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FitToHeight()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x * 1f,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);

        if (name == "Background" || name == "Foreground")
        {
            transform.localScale = new Vector3(
                transform.localScale.x * 1.04f,
                transform.localScale.y, 1);
        }
    }
}
