using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSprite : MonoBehaviour
{
    public bool active { get; private set; } = false;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private bool hasTarget = false;

    private float speed = 2.0f;

    private float width;
    private float height;

    public void Init()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public void Move(float rightBound)
    {
        transform.Translate(speed * direction.normalized * Time.deltaTime);
        Debug.Log(transform.position.y);
        Debug.Log(targetPos.y);
        if (hasTarget &&

            transform.position.y < targetPos.y)
        {
            Deactivate();
        }
        else if (transform.position.x > rightBound + width / 2)
        {
            Deactivate();
        }
    }

    public void Spawn(float speed, float leftBound, float yPos, float maxSpeed, float minSpeed)
    {
        hasTarget = false;

        active = true;

        gameObject.SetActive(true);

        this.speed = speed;

        direction = new Vector3(1.0f, 0.0f, 0.0f);

        // Faster moving clouds will be in front of slower moving clods
        float zPos = minSpeed + (maxSpeed - speed);

        transform.position = new Vector3(leftBound - width / 2, yPos, zPos);
    }

    public void SpawnTarget(float speed, Vector2 startPos, Vector2 endPos)
    {
        hasTarget = true;

        active = true;

        gameObject.SetActive(true);

        this.speed = speed;
        direction = endPos - startPos;
        // The object will move between the starting and ending position
        transform.position = new Vector3(startPos.x, startPos.y, 0);
        targetPos = endPos;
    }

    public void SpawnDepth(float speed, float leftBound, float yPos, float minY, float maxY)
    {
        hasTarget = false;

        active = true;

        gameObject.SetActive(true);

        this.speed = speed;

        direction = new Vector3(1.0f, 0.0f, 0.0f);

        // Objects higher on the scale will be smaller.
        float yRatio = (yPos - minY) / (maxY - minY);
        float maxOffset = 0.4f;
        Vector3 offset = Vector3.one * (maxOffset * yRatio);
        transform.localScale = Vector3.one - offset;

        transform.position = new Vector3(leftBound - width / 2, yPos, 0);
    }
}
