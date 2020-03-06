using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickEnemy : Enemy
{
    [SerializeField]
    private HingeJoint2D hinge;
    [SerializeField]
    private Rigidbody2D leftHand;
    [SerializeField]
    private Rigidbody2D rightHand;

    private Rigidbody2D hand;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (velocity.x < 0)
            hand = leftHand;
        else hand = rightHand;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Set the hinge correctly
            hand.position = collision.contacts[0].point;
            hinge.transform.SetParent(collision.transform);
            hinge.transform.position = hand.position;
            hinge.connectedBody = hand;
            hinge.connectedAnchor = hinge.transform.InverseTransformPoint(hand.position);

            velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            hand.velocity = Vector2.zero;
            move = false;
            gameObject.layer = LayerMask.NameToLayer("EnemyLimb");
            collision.gameObject.GetComponent<Pole>().AddMass(mass);
            collision.gameObject.GetComponent<Pole>().PlayStickSound();
        }
    }


    public void RemoveEnemy()
    {
        gameObject.GetComponentInParent<Pole>().AddMass(-mass);
    }

    private void OnDestroy()
    {
        if (hinge != null)
            Destroy(hinge);
    }

}
