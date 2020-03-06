﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickEnemy : Enemy, IBeginDragHandler, IDragHandler
{
    private const float MAX_DRAG_SQUARED = 30;

    [SerializeField]
    private HingeJoint2D hinge;
    [SerializeField]
    private Rigidbody2D leftHand;
    [SerializeField]
    private Rigidbody2D rightHand;

    private Rigidbody2D hand;
    private Vector2 offset;
    private bool canDrag;

    protected override void Awake()
    {
        base.Awake();
        canDrag = false;
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

            //Reset physics
            velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
            hand.velocity = Vector2.zero;
            move = false;
            canDrag = true;
            gameObject.layer = LayerMask.NameToLayer("EnemyLimb");

            Pole pole = collision.gameObject.GetComponent<Pole>();
            pole.AddForce(force / 4); //smaller force for the sticky enemies
            pole.AddMass(mass);
            pole.PlayStickSound();
        }
    }

    public void RemoveEnemy()
    {
        GameManager.Instance.Pole.AddMass(-mass);
        hinge.connectedBody = null;
        canDrag = false;
    }

    private void OnDestroy()
    {
        //Ensure hinge is destroyed
        if (hinge != null)
            Destroy(hinge.gameObject);
    }

    /// <summary>
    /// Calculate the offset for dragging sticky enemies off the pole
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag)
            offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    /// <summary>
    /// Drag the sticky enemy off of the pole
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
            return;

        //Set position of limb
        Vector2 point = Camera.main.ScreenToWorldPoint(eventData.position);
        eventData.pointerDrag.GetComponent<Rigidbody2D>().position = point + offset;

        //Remove limb if dragged far enough
        Vector2 diff = (Vector2)hinge.transform.position - point;
        float sqrMag = diff.sqrMagnitude;
        if (sqrMag > MAX_DRAG_SQUARED)
        {
            RemoveEnemy();
            rb.AddForce(-diff.normalized * sqrMag * 100);
        }
    }
}