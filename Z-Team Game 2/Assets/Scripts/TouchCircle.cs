using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCircle : MonoBehaviour
{
    private const int NUM_VERTICES = 40;
    private const float MAX_RADIUS = 0.7f;
    private const float CIRCLE_SPEED = 3.5f;

    private Circle circle;

    public void Init(Vector2 position)
    {
        circle = new Circle(0.0f, NUM_VERTICES);
        
        transform.position = new Vector3(position.x, position.y, 0.0f);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        lineRenderer.positionCount = NUM_VERTICES;
        lineRenderer.SetPositions(circle.GetPoints());
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.black;
    }

    // Update is called once per frame
    public bool OnUpdate()
    {
        float radius = circle.GetRadius();
        radius += CIRCLE_SPEED * Time.deltaTime;

        if (circle.GetRadius() > MAX_RADIUS) return true;

        circle.SetRadius(radius);
        GetComponent<LineRenderer>().SetPositions(circle.GetPoints());
        return false;
    }
}
