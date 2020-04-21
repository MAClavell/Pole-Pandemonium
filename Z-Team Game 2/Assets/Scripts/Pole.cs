using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pole : MonoBehaviour
{
    public float Rotation { get; private set; }
    public bool hapticFeedback { get; private set; } = true;
    public bool visualFeedback { get; private set; } = true;
	public SpriteRenderer SpriteRenderer { get; private set; }

	private const float RESISTANCE = 1.0f;
    private const float STARTING_FORCE = 120.0f;


    // Circle click feedback related variables
    public Transform circleParent;
    private GameObject circleObj;
    private List<TouchCircle> circles = new List<TouchCircle>();
    private float height;
    private float totalForce;
    private float rotationalVelocity;
    private float mass;

    private AudioSource tapSound;
    private AudioSource hitPole;
    private AudioSource stickPole;
    private AudioSource swipedOff;

#if !UNITY_EDITOR && UNITY_ANDROID
    private int hapticFeedbackKey;
    private AndroidJavaObject currentActivity;
#endif

    void Awake()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        circleObj = Resources.Load<GameObject>("Prefabs/Circle");

        tapSound = GameObject.Find("tapSound").GetComponent<AudioSource>();
        hitPole = GameObject.Find("enemyHitPole").GetComponent<AudioSource>();
        stickPole = GameObject.Find("enemyStickPole").GetComponent<AudioSource>();
        swipedOff = GameObject.Find("enemySwipedOff").GetComponent<AudioSource>();

#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass hfc = new AndroidJavaClass("android.view.HapticFeedbackConstants");

        hapticFeedbackKey = hfc.GetStatic<int>("VIRTUAL_KEY");

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        currentActivity.Call("setHapticFeedbackEnabled", true);
#endif
    }

    private void Start()
    {
        height = SpriteRenderer.sprite.rect.height;
    }

    /// <summary>
    /// Reset the pole to its starting state
    /// </summary>
    public void Init()
    {
        //Reset vars
        mass = 1.0f;
        totalForce = 0;
        Rotation = 0;
        rotationalVelocity = 0;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        // Add an initial force to make the pole fall for testing purposes
        AddForce((Random.value < .5 ? 1 : -1) * STARTING_FORCE);
    }

    public void OnUpdate()
    {
        if (!GameManager.Instance.IsPlaying)
            return;

        Touch[] touches = null;
        //Detect tap input
        //Sorry for the odd preprocessors, blame unity for not allowing touch simulation in the editor
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            {
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else
        touches = Input.touches;
        if (Input.touchCount > 0)
        {
            Touch? touch = null;
            foreach (Touch t in touches)
            {
                //Check for a valid touch
                if(t.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    touch = t;
                    break;
                }
            }

            if (touch.HasValue)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(touch.Value.position);
#endif
                float invertScalar = Config.Invert ? -1 : 1;
                float angle = 0;

                //Pole angle based controls
                if (Config.ControlScheme == ControlScheme.Angle)
                {
                    //Calculate the angle between the click position and the top of the pole
                    Vector2 top = new Vector2(0, 1);
                    if(Rotation > 65)
                    {
                        top = Quaternion.AngleAxis(-15, Vector3.forward) * top;
                    }
                    else if(Rotation < -65)
                    {
                        top = Quaternion.AngleAxis(15, Vector3.forward) * top;
                    }

                    Vector2 angleVec = transform.TransformPoint(top);
                    angle = Vector2.Angle(angleVec, position) * Mathf.Sign(position.x * angleVec.y - position.y * angleVec.x);

                    if (hapticFeedback)
                    {
                        // Vibrate();
                    }
                    if (visualFeedback)
                    {
                        CreateTouchCircle(position);
                    }
#if UNITY_EDITOR
                    Debug.DrawLine(Vector3.zero, angleVec * 20, Color.red, 0.5f);
#endif
                }
                //Screen based controls
                else
                {
                    if (position.x > 0)
                        angle = 1;
                    else angle = -1;
                }

                //Touch was to the left of the pole
                if (angle > 0)
                {
                    //Debug.Log("Left touch");
                    AddForce(1200 * invertScalar);

                }
                //Touch was to the right of the pole
                else
                {
                    //Debug.Log("Right touch");
                    AddForce(-1200 * invertScalar);
                }
                tapSound.Play();
            }
        }

        //Apply physics
        RotatePole();
        totalForce = 0.0f;

        if (visualFeedback)
        {
            UpdateCircles();
        }
    }

    private bool Vibrate()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        bool result = currentActivity.Call<bool>("performHapticFeedback", hapticFeedbackKey);

        return result;
#else
        return false;
#endif
    }

    private void CreateTouchCircle(Vector2 position)
    {
        GameObject obj = Instantiate(circleObj, circleParent);
        TouchCircle newCircle = obj.GetComponent<TouchCircle>();
        newCircle.Init(position);
        circles.Add(newCircle);
    }

    private void UpdateCircles()
    {
        if (circles.Count <= 0) return; 

        for (int i = circles.Count - 1; i >= 0; i--)
        {
            if (circles[i].OnUpdate())
            {
                Destroy(circles[i].gameObject);

                circles.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Calculate the force due to gravity then rotate the pole based on all of the acting forces.
    /// </summary>
    private void RotatePole()
    {
        CalculateRotationalVelocity();

        Rotation += rotationalVelocity * Time.deltaTime;
        Rotation = Mathf.Clamp(Rotation, -90.0f, 90.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Rotation);
    }

    /// <summary>
    /// Calculate the rotational velocity of the pole
    /// </summary>
    private void CalculateRotationalVelocity()
    {
        float gravAcceleration = Mathf.Sign(transform.eulerAngles.z) * GameManager.GRAVITY * RESISTANCE * (Mathf.Sin(Mathf.Abs(transform.eulerAngles.z) * Mathf.Deg2Rad));

        float rotationalAcceleration = gravAcceleration + totalForce / mass;

        rotationalVelocity += rotationalAcceleration * Time.deltaTime;
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
    /// <param name="m">The amount of mass to add</param>
    /// <param name="vPos">How high up the pole the mass is </param>
    /// <param name="offSet">The distance offset, perpendicular to the pole, that the mass is applied</param>
    /// <param name="side">The direction of the offset. Negative is left side, positive is right side</param>
    public void AddMass(float m, float vPos = 0.5f, int side = 1)
    {
        mass += m;
        PlayStickSound();
    }

    public void PlayHitSound()
    {
        hitPole.Play();
    }
    public void PlayStickSound()
    {
      stickPole.Play();
    }
    public void PlaySwipedSound()
    {
        swipedOff.Play();
    }
}
