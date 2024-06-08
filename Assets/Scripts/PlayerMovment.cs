using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float mMoveSpeed = 5f;        // Speed of the player movement
    public float mJumpForce = 5f;        // Force applied when the player jumps
    public bool mIsGrounded;             // Flag to check if the player is on the ground

    private Rigidbody mRigidBody;               // Reference to the Rigidbody component

    void Start()
    {
        // Get the Rigidbody component attached to this game object
        mRigidBody = GetComponent<Rigidbody>();
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
