using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private GameObject mTarget = null;
    [SerializeField]
    private float mSpeed = 10.0f;
    [SerializeField]
    private Animator mAnimator = null;

    private void Start()
    {
        mAnimator.SetFloat("Speed", mSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, mTarget.transform.position, mSpeed * Time.fixedDeltaTime);
    }
}
