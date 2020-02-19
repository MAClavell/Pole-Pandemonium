using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class FitToScreenHeight : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FitToHeight()
    {
        Vector2 resolution = new Vector2(Screen.width, Screen.height);
        float aspectRatio = resolution.x / resolution.y;

        float camHeight = 100 * Camera.main.orthographicSize * 2;
        float camWidth = camHeight * aspectRatio;

        float newHeight = camHeight / sr.sprite.rect.height;
        float newWidth = camWidth / sr.sprite.rect.width;

        transform.localScale = new Vector3(newWidth, newHeight, 1);


    }
}
