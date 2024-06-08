using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float mMoveSpeed = 5f;        // Speed of the player movement
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

    [SerializeField]
    private Animator mAnimator = null;

    private float mHorizontalInput = 0;
    private float mVerticalInput = 0;

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
        {
            mTop.gameObject.SetActive(!mInLight);
            mRight.gameObject.SetActive(!mInLight);
            mBottom.gameObject.SetActive(!mInLight);
            mLeft.gameObject.SetActive(!mInLight);
        }

        // Animations
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                mAnimator.SetTrigger("Attack");
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void GetInput()
    {
        // Get input from the horizontal and vertical axes (WASD or arrow keys)
        mHorizontalInput = Input.GetAxis("Horizontal");
        mVerticalInput = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        // Create a new Vector3 for the player's movement
        var movement = transform.forward * mVerticalInput + transform.right * mHorizontalInput;

        // Apply the movement to the Rigidbody's velocity
        mRigidBody.AddForce(movement.normalized * mMoveSpeed * Time.deltaTime);
    }
}
