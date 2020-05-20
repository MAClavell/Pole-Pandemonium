using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainMover : MonoBehaviour
{
    const float MIN_DELAY = 0.05f;
    const float MAX_DELAY = 0.012f;

    const float MIN_SPEED = 6.5f;
    const float MAX_SPEED = 8.5f;

    private List<MovingSprite> sprites = new List<MovingSprite>();

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

        minStartX = -worldScreenWidth / 2;
        maxStartX = worldScreenWidth / 2;

        foreach (Transform child in transform)
        {
            sprites.Add(child.gameObject.GetComponent<MovingSprite>());
        }

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

                currentSprite = (currentSprite + 2) % sprites.Count;

                if (!sprites[currentSprite].active)
                {
                    float speed = Random.Range(MIN_SPEED, MAX_SPEED);
                    float startX = Random.Range(minStartX, maxStartX);
                    Vector2 startPos = new Vector2(startX, startY);
                    Vector2 endPos = new Vector2(startX, targetY);
                    sprites[currentSprite].SpawnTarget(speed, startPos, endPos);
                }
                if (!sprites[currentSprite + 1].active)
                {
                    float speed = Random.Range(MIN_SPEED, MAX_SPEED);
                    float startX = Random.Range(minStartX, maxStartX);
                    Vector2 startPos = new Vector2(startX, startY);
                    Vector2 endPos = new Vector2(startX, targetY);
                    sprites[currentSprite + 1].SpawnTarget(speed, startPos, endPos);
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