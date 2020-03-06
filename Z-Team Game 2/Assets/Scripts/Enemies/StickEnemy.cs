using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickEnemy : Enemy
{
    [SerializeField]
    private GameObject hinge;

    protected override void Awake()
    {
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
            //gameObject.transform.SetParent(collision.transform);
            //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            //velocity = Vector3.zero;
            //collision.gameObject.GetComponent<Pole>().AddMass(mass);
            //collision.gameObject.GetComponent<Pole>().PlayStickSound();
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
