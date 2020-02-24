using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitToScreenHeight : MonoBehaviour
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
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        float aspectRatio = resolution.x / resolution.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);


    }
}
