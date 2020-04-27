using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMover : MonoBehaviour
{
    const float MIN_DELAY = 3.0f;
    const float MAX_DELAY = 8.0f;

    const float MIN_SPEED = 1.5f;
    const float MAX_SPEED = 2.3f;

    public List<MovingSprite> sprites;

    private int currentSprite = 0;

    private float leftBound;
    private float rightBound;

    private float spawnTimer;
    private float spawnDelay;

    private float startY;
    private float targetY;

    private float minStartX;
    private float maxStartX;
    private float minTargetX;
    private float maxTargetX;

    // Start is called before the first frame update
    void Awake()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        rightBound = worldScreenWidth / 2;
        leftBound = -rightBound;

        startY = worldScreenHeight * 1.1f; ;
        targetY = 0.0f - worldScreenHeight * 0.1f;

        minStartX = -worldScreenWidth / 2 * 0.9f;
        maxStartX = worldScreenWidth / 2 * 1.1f;
        minTargetX = -worldScreenWidth / 2 * 0.9f;
        maxTargetX = worldScreenWidth / 2 * 1.1f;

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
                    Vector2 startPos = new Vector2(Random.Range(minStartX, maxStartX), startY);
                    Vector2 endPos = new Vector2(Random.Range(minTargetX, maxTargetX), targetY);
                    sprites[currentSprite].SpawnTarget(speed, startPos, endPos);
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