using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    float accelaration;
    float velocity;
    float mass;
    float rot;
    Quaternion previousRotation;
    Vector3 angularVelocity;

    void Awake()
    {
        accelaration = -1000;
        velocity = 0;
        mass = 1;
        rot = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {

    }

    private void Update()
    {

    }

    void AddForce(float force)
    {
        accelaration += force / mass;
    }
}
