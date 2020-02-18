using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public float CurrentRotation { get; private set; }

    private const float RESISTANCE = 1.0f;
    private const float STARTING_FORCE = 100.0f;

    private SpriteRenderer spriteRenderer;
    private float height;
    private float totalForce;
    private float rotation;
    private float mass;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        height = spriteRenderer.sprite.rect.height;
        Debug.Log($"Pole Height: {height}");
    }

    /// <summary>
    /// Reset the pole to its starting state
    /// </summary>
    public void Init()
    {
        //Reset vars
        mass = 10.0f;
        totalForce = 0;
        rotation = 0;
        CurrentRotation = 0;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        // Add an initial force to make the pole fall for testing purposes
        AddForce((Random.value < .5 ? 1 : -1) * STARTING_FORCE);
    }

    public void Update()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        //Detect tap input
        //Sorry for the odd preprocessors, blame unity for not allowing touch simulation in the editor
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            {
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
#endif
                //Calculate the angle between the click position and the top of the pole
                Vector2 top = transform.TransformPoint(new Vector2(0, 1));
                float angle = Vector2.Angle(top, position) * Mathf.Sign(position.x * top.y - position.y * top.x);

                //Touch was to the left of the pole
                if(angle > 0)
                {
                    Debug.Log("Left touch");
                    AddForce(100);
                }
                //Touch was to the right of the pole
                else
                {
                    Debug.Log("Right touch");
                    AddForce(-100);
                }
            }
        }

        //Apply physics
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
        totalForce += force / mass;
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

        float gravForce = direction * GameManager.GRAVITY * RESISTANCE * (Mathf.Sin((Mathf.Abs(transform.eulerAngles.z) * Mathf.Deg2Rad)));
        totalForce += gravForce;

        rotation += Mathf.Sign(totalForce) * Mathf.Pow(totalForce, 2) * Time.deltaTime;

        CurrentRotation += rotation * Time.deltaTime;
        CurrentRotation = Mathf.Clamp(CurrentRotation, -90.0f, 90.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, CurrentRotation);
    }
}
