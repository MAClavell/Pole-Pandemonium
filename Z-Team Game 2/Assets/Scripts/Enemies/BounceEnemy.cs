using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Pole>().AddForce(force);
            rb.velocity = Vector2.zero;
            velocity.x = -velocity.x;
            collision.gameObject.GetComponent<Pole>().PlayHitSound();
            Debug.Log("ENEMYBOUNCE");
        }
    }



}
