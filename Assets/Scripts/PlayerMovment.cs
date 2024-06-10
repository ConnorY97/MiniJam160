using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float mMaxSpeed = 10f;        // Speed of the player movement
    [SerializeField]
    private float mAcceleration = 5f;        // Speed of the player movement
    [SerializeField]
    private float mFriction = 10f;
    [SerializeField]
    private Slider mHealthBar = null;
    [SerializeField]
    private float mHealth = 100.0f;
    public float Health
    {
        get { return mHealth; }
    }
    [SerializeField]
    private float mDamage = 5.0f;
    [SerializeField]
    private float mDamageDelay = 0.25f;
    [SerializeField]
    private bool mInLight = false;
    public bool InLight
    {
        set { mInLight = value; }
    }

    [SerializeField]
    private Animator mAnimator = null;

    [SerializeField]
    private Volume mVolume = null;

    [SerializeField]
    private float mLungeStrength = 1000.0f;

    private float mHorizontalInput = 0;
    private float mVerticalInput = 0;

    private float mDamageTime = 0.0f;

    private Rigidbody mRigidBody;               // Reference to the Rigidbody component

    private bool mInputEnabled = true;
    public bool InputEnabled
    {
        get { return mInputEnabled; }
        set { mInputEnabled = value; }
    }

    void Start()
    {
        // Get the Rigidbody component attached to this game object
        mRigidBody = GetComponent<Rigidbody>();

        mDamageTime = Time.time + mDamageDelay;
    }

    void Update()
    {
        GetInput();

        // Outside of the light take damage
        if (mInLight == false && mDamageTime < Time.time)
        {
            mHealth -= mDamage;
            mDamageTime = Time.time + mDamageDelay;
        }
        // Inside of the light heal
        else if (mInLight == true && mDamageTime < Time.time)
        {
            if (mHealth < 100.0f)
            {
                mHealth += mDamage;
            }
            mDamageTime = Time.time + mDamageDelay;
        }

        mHealthBar.value = mHealth;

        // UI
        // Grab the vigenette post proccessing effect and set the colour based on if the player is in the light or not
        {
            mVolume.profile.TryGet(out Vignette vignette);
            if (mInLight)
            {
                vignette.intensity.value = 0.0f;
            }
            else
            {
                vignette.intensity.value = 0.4f;
            }
        }

        // Animations
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                mAnimator.SetTrigger("Attack");

                // Lunge forward
                mRigidBody.AddForce(transform.forward * mLungeStrength);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void GetInput()
    {
        if (mInputEnabled)
        {
            // Get input from the horizontal and vertical axes (WASD or arrow keys)
            mHorizontalInput = Input.GetAxisRaw("Horizontal");
            mVerticalInput = Input.GetAxisRaw("Vertical");
        }
    }

    private void Move()
    {
        Vector3 velocity = mRigidBody.velocity;
        // Create a new Vector3 for the player's movement
        var movement = transform.forward * mVerticalInput + transform.right * mHorizontalInput;

        Vector3 movementForce = movement.normalized * (mAcceleration + mFriction);
        Vector3 frictionForce = -velocity.normalized * mFriction;

        Vector3 force = Vector3.zero;
        force += movementForce;
        force += frictionForce;

        velocity += force * Time.fixedDeltaTime;

        if (Vector3.Dot(velocity, force) < 0f && velocity.magnitude <= 1f)
            velocity = Vector3.zero;

        velocity = Vector3.ClampMagnitude(velocity, mMaxSpeed);
        mRigidBody.velocity = velocity;
    }
}