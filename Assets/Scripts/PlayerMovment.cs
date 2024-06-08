using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float mMoveSpeed = 5f;        // Speed of the player movement
    [SerializeField]
    private float mJumpForce = 5f;        // Force applied when the player jumps
    [SerializeField]
    private bool mIsGrounded = false;             // Flag to check if the player is on the ground
    [SerializeField]
    private Slider mHealthBar = null;
    [SerializeField]
    private float mHealth = 100.0f;
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
    private Image mLeft = null;
    [SerializeField]
    private Image mRight = null;
    [SerializeField]
    private Image mTop = null;
    [SerializeField]
    private Image mBottom = null;

    private float mDamageTime = 0.0f;

    private Rigidbody mRigidBody;               // Reference to the Rigidbody component

    void Start()
    {
        // Get the Rigidbody component attached to this game object
        mRigidBody = GetComponent<Rigidbody>();

        mDamageTime = Time.time + mDamageDelay;
    }

    void Update()
    {
        // Get input from the horizontal and vertical axes (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create a new Vector3 for the player's movement
        Vector3 movement = new Vector3(moveX * mMoveSpeed, mRigidBody.velocity.y, moveZ * mMoveSpeed);

        // Apply the movement to the Rigidbody's velocity
        mRigidBody.velocity = movement;

        // Check for jump input (space bar) and if the player is grounded
        if (Input.GetButtonDown("Jump") && mIsGrounded)
        {
            mRigidBody.AddForce(new Vector3(0f, mJumpForce, 0f), ForceMode.Impulse);
        }

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
        {
            mTop.gameObject.SetActive(!mInLight);
            mRight.gameObject.SetActive(!mInLight);
            mBottom.gameObject.SetActive(!mInLight);
            mLeft.gameObject.SetActive(!mInLight);
        }
    }

    // Check if the player is colliding with the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            mIsGrounded = true;
        }
    }

    // Check if the player has left the ground
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            mIsGrounded = false;
        }
    }
}
