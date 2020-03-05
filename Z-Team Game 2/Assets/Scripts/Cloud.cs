using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public bool active { get; private set; } = false;

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
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (transform.position.x > rightBound + width / 2)
        {
            Deactivate();
        }
    }

    public void Spawn(float speed, float leftBound, float yPos, float MAX_SPEED, float MIN_SPEED)
    {
        active = true;

        gameObject.SetActive(true);

        this.speed = speed;

        // Faster moving clouds will be in front of slower moving clods
        float zPos = MIN_SPEED + (MAX_SPEED - speed);

        transform.position = new Vector3(leftBound - width / 2, yPos, zPos);
    }
}
