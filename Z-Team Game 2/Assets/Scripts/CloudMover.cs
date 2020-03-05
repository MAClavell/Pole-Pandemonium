using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    const float MIN_DELAY = 2.0f;
    const float MAX_DELAY = 5.0f;

    const float MIN_SPEED = 0.5f;
    const float MAX_SPEED = 1.5f;

    public List<Cloud> clouds;

    private int currentCloud = 0;

    private float leftBound;
    private float rightBound;

    private float spawnTimer;
    private float spawnDelay;

    private float minY;
    private float maxY;

    // Start is called before the first frame update
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        rightBound = worldScreenWidth / 2;
        leftBound = -rightBound;

        minY = worldScreenHeight * 0.6f;
        maxY = worldScreenHeight * 1.0f;

        foreach (Cloud cloud in clouds)
        {
            cloud.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing &&
            clouds.Count > 0)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnDelay)
            {
                spawnTimer = 0;
                spawnDelay = Random.Range(MIN_DELAY, MAX_DELAY);

                if (clouds[currentCloud].active)
                {
                    return;
                }
                else
                {
                    float speed = Random.Range(MIN_SPEED, MAX_SPEED);
                    float yPos = Random.Range(minY, maxY);

                    clouds[currentCloud].Spawn(speed, leftBound, yPos, MAX_SPEED, MIN_SPEED);

                    currentCloud = (currentCloud + 1) % clouds.Count;
                    Debug.Log(currentCloud);
                }
            }

            MoveClouds();
        }
    }

    public void ResetClouds()
    {
        foreach (Cloud cloud in clouds)
        {
            cloud.Deactivate();
        }
    }

    void MoveClouds()
    {
        foreach (Cloud cloud in clouds)
        {
            if (cloud.active)
            {
                cloud.Move(rightBound);
            }
        }
    }
}