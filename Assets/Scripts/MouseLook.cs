using TreeEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Member variables
    [SerializeField]
    private float mMouseSensitivity = 100f;
    [SerializeField] private Transform mPlayerBody;

    [SerializeField]
    private Transform mCamPos = null;

    private float mXRot = 0f;
    private float mYRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mMouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mMouseSensitivity * Time.deltaTime;

        // Calculate the new rotation
        mXRot -= mouseY;
        mXRot = Mathf.Clamp(mXRot, -90f, 90f);

        mYRot += mouseX;

        // Apply rotation to the camera and the player body
        transform.localRotation = Quaternion.Euler(mXRot, mYRot, 0f);

        mPlayerBody.rotation = Quaternion.Euler(0, mYRot, 0);

        transform.position = mCamPos.position;
    }
}
