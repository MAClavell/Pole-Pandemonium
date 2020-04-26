using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMover : MonoBehaviour
{
    const float MIN_DELAY = 5.0f;
    const float MAX_DELAY = 10.0f;

    const float MIN_SPEED = 0.7f;
    const float MAX_SPEED = 0.9f;

    public List<MovingSprite> sprites;

    private int currentSprite = 0;

    private float leftBound;
    private float rightBound;

    private float spawnTimer;
    private float spawnDelay;

    private float minY;
    private float maxY;

    // Start is called before the first frame update
    void Awake()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        rightBound = worldScreenWidth / 2;
        leftBound = -rightBound;

        minY = worldScreenHeight * 0.03f;
        maxY = worldScreenHeight * 0.24f;

        foreach (MovingSprite sprite in sprites)
        {
            sprite.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing &&
            sprites.Count > 0)
        {
            spawnTimer += Time.deltaTime;

            MoveSprites();

            if (spawnTimer > spawnDelay)
            {
                spawnTimer = 0;
                spawnDelay = Random.Range(MIN_DELAY, MAX_DELAY);

                currentSprite = (currentSprite + 1) % sprites.Count;

                if (sprites[currentSprite].active)
                {
                    return;
                }
                else
                {
                    float speed = Random.Range(MIN_SPEED, MAX_SPEED);
                    float yPos = Random.Range(minY, maxY);

                    sprites[currentSprite].SpawnDepth(speed, leftBound, yPos, minY, maxY);
                }
            }
        }
    }

    public void ResetClouds()
    {
        foreach (MovingSprite sprite in sprites)
        {
            sprite.Deactivate();
        }
    }

    private void MoveSprites()
    {
        foreach (MovingSprite sprite in sprites)
        {
            if (sprite.active)
            {
                sprite.Move(rightBound);
            }
        }
    }
}