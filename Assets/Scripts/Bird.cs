using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform target = null;
    [SerializeField]
    private float mMinSpeed, mMaxSpeed = 0.0f;
    [SerializeField]
    private float mMinY, mMaxY, mMinX, mMaxX;
    [SerializeField]
    private float mMinDistToBoundary = 0.0f;
    [SerializeField]
    private Animator mAnimator = null;

    [SerializeField]
    private float mMaxSteeringForce = 0.0f;
    [SerializeField]
    private float mCohesionFactor, mAlignmentFactor, mSeparationFactor, mTargetSeekingFactor, mBoundaryFactor = 0.0f;

    public float perceptionRadius, separationRadius;

    [HideInInspector]
    public Vector3 offsetToCenterOfFlock, flockAlignment, flockSeparation = Vector3.zero;

    public Vector3 velocity => mVelocity;
    private Vector3 mVelocity = Vector3.forward;

    private void Start()
    {
        mAnimator.SetFloat("Speed", mMaxSpeed);
    }

    public void UpdateBoid()
    {
        Vector3 cohesionForce = CalculateSteeringForce(offsetToCenterOfFlock) * mCohesionFactor;
        Vector3 alignmentForce = CalculateSteeringForce(flockAlignment) * mAlignmentFactor;
        Vector3 separationForce = CalculateSteeringForce(flockSeparation) * mSeparationFactor;

        Vector3 offsetToTarget = target.position - transform.position;
        Vector3 targetSeekingForce = CalculateSteeringForce(offsetToTarget) * mTargetSeekingFactor;

        // some fucky boundary force
        float yOffsetToBoundary = Mathf.Min(Mathf.Abs(mMinY - transform.position.y), Mathf.Abs(mMaxY - transform.position.y));
        Vector3 yBoundaryForce = Vector3.zero;

        float xOffsetToBoundary = Mathf.Min(Mathf.Abs(mMinX - transform.position.x), Mathf.Abs(mMaxX - transform.position.x));
        Vector3 xBoundaryForce = Vector3.zero;

        if (yOffsetToBoundary <= mMinDistToBoundary)
        {
            float inverseFract = 1.0f - yOffsetToBoundary / mMinDistToBoundary;
            float dir = Mathf.Sign(((mMinY + mMaxY) / 2) - transform.position.y);

            yBoundaryForce = (dir * Vector3.up) * mMaxSteeringForce * mBoundaryFactor;
        }

        if (xOffsetToBoundary <= mMinDistToBoundary)
        {
            float inverseFract = 1.0f - xOffsetToBoundary / mMinDistToBoundary;
            float dir = Mathf.Sign(((mMinX + mMaxX) / 2) - transform.position.x);

            xBoundaryForce = (dir * Vector3.right) * mMaxSteeringForce * mBoundaryFactor;
        }

        Vector3 acceleration = cohesionForce + alignmentForce + separationForce + targetSeekingForce + xBoundaryForce + yBoundaryForce;

        mVelocity += acceleration * Time.fixedDeltaTime;
        float speed = Mathf.Clamp(mVelocity.magnitude, mMinSpeed, mMaxSpeed);
        mVelocity = mVelocity.normalized * speed;

        transform.position += mVelocity * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(new Vector3(mVelocity.x, 0f, mVelocity.z).normalized, Vector3.up);
    }

    private Vector3 CalculateSteeringForce(Vector3 desiredVelocity)
    {
        Vector3 force = desiredVelocity.normalized * mMaxSpeed - mVelocity;
        force = Vector2.ClampMagnitude(force, mMaxSteeringForce);
        return force;
    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, mTarget.transform.position, mSpeed * Time.fixedDeltaTime);
    //}

    public void Dead()
    {
        GameManager.Instance.RemoveBird(this);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, perceptionRadius);
    }
}