using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public float currentRotation { get; private set; }

    private float totalForce;

    private float resistance;

    private float mass;

    private float height;

    void Awake()
    {
        resistance = 1.0f;

        mass = 1.0f;

        height = gameObject.GetComponent<SpriteRenderer>().sprite.rect.height;

        Debug.Log("Pole Height: " + height);
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init()
    {
        // Add an initial force to make the pole fall for testing purposes
        AddForce(10.0f);
    }

    public void Update()
    {
        RotatePole();
        totalForce = 0.0f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="force">The amount of the force that is being applied. Negative is counter clockwise, Positive is clockwise</param>
    /// <param name="vPos">At what height on the pole is it being applied to</param>
    public void AddForce(float force, float vPos = 1.0f)
    {
        totalForce += force;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">The position of the collision</param>
    /// <param name="mass">The mass of the colliding object</param>
    /// <param name="velocity">The Velocity of the colliding object</param>
    /// <param name="attach">Does the colliding object stick to the pole?</param>
    public void Collide(Vector2 position, float mass, Vector2 velocity, bool attach = false)
    {
        // Assuming that if the x velocity is position, then the pole should be pushed clockwise and vice versa 
        int direction = velocity.x < 0 ? -1 : 1;

        // Only care about the force that is perpendicular to the pole.
        float force = direction * 5.0f;

        // Find where along the pole the collision happend based on the position of the collision
        float vPos = 0.5f;

        AddForce(force, vPos);

        if (attach)
        {
            AddMass(mass);
        }
    }

    /// <summary>
    /// Add a mass to the pole
    /// </summary>
    /// <param name="mass">The amount of mass to add</param>
    /// <param name="vPos">How high up the pole the mass is </param>
    /// <param name="offSet">The distance offset perpendicular to the pole the mass is applied</param>
    /// <param name="side">The direction of the offset. Negative is left side, positive is right side</param>
    public void AddMass(float mass, float vPos = 0.5f, float offSet = 0.0f, int side = 1)
    {
        
    }

    /// <summary>
    /// Calculate the force due to gravity then rotate the pole based on all of the acting forces.
    /// </summary>
    private void RotatePole()
    {
        int direction = transform.eulerAngles.z < 0 ? -1 : 1;

        float gravForce = direction * GameManager.GRAVITY * resistance * (Mathf.Sin((Mathf.Abs(transform.eulerAngles.z) * Mathf.Deg2Rad)));

        //Debug.Log(gravForce);

        totalForce += gravForce;

        float rotation = Mathf.Pow((totalForce / mass), 2) * Time.deltaTime;

        currentRotation += rotation;

        currentRotation = Mathf.Clamp(currentRotation, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentRotation);
    }
}
