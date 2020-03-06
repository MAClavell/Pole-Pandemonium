using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const int X_VELOCITY_MIN = 900;
    private const int X_VELOCITY_MAX = 1050;
    private const int Y_VELOCITY_MIN = 7;
    private const int Y_VELOCITY_MAX = 17;

    public int Side { get; set; }
    protected Vector2 velocity;
    protected Rigidbody2D rb;
    protected float force;
    protected float mass;
    protected bool move;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Set when enemy is spawned in EnemyManager
        //Left = 0, Right = 1
        switch (Side)
        {
            case 0:
                //Velocity to the right
                velocity = new Vector2(Random.Range(X_VELOCITY_MIN, X_VELOCITY_MAX), Random.Range(Y_VELOCITY_MIN, Y_VELOCITY_MAX));
                force = -6.0f * velocity.x;
                mass = .1f;
                break;
            case 1:
                //Velocity to the left
                velocity = new Vector2(Random.Range(-X_VELOCITY_MIN, -X_VELOCITY_MAX), Random.Range(Y_VELOCITY_MIN, Y_VELOCITY_MAX));
                force = -6.0f * velocity.x;
                mass = .1f;
                break;
        }
        move = true;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        if(move)
            rb.velocity = new Vector2(velocity.x * Time.fixedDeltaTime, (rb.velocity.y * Time.fixedDeltaTime) + velocity.y);

        if (transform.position.y < -3.0f || transform.position.y > (Camera.main.orthographicSize * 2) + 3)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);

}
