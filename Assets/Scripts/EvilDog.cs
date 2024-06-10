using System.IO;
using UnityEngine;

public class EvilDog : MonoBehaviour
{
    private GameObject mTarget = null;

    [SerializeField]
    private float mSpeed = 8.0f;

    [SerializeField]
    public float mAttackRange = 5.0f;

    [SerializeField]
    private Animator mAnimator = null;

    // Start is called before the first frame update
    void Start()
    {
        FindTarget();

        // Make the bear run
        mAnimator.SetBool("Run Forward", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Make sure we do this first
        // If another dog gets our target, find antother
        if (mTarget == null)
        {
            FindTarget();

            // If we can't find a target that means there are no birds left
            // Die
            if (mTarget == null)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Once we are in range we can start moving up towards the bird
            if (Vector3.Distance(transform.position, mTarget.transform.position) < mAttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, mTarget.transform.position, mSpeed * Time.fixedDeltaTime);

                // Stop running and jump
                mAnimator.SetBool("Run Forward", false);
                mAnimator.SetTrigger("Attack3");
            }
            // Move along the ground till we get close
            else
            {
                Vector3 targetHeight = new Vector3(mTarget.transform.position.x, transform.position.y, mTarget.transform.position.z);

                transform.position = Vector3.MoveTowards(transform.position, targetHeight, mSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void FindTarget()
    {
        var targets = GameObject.FindGameObjectsWithTag("Bird");

        float nearest = float.MaxValue;
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < nearest)
            {
                nearest = dist;
                mTarget = target;
            }
        }
    }

    public void Dead()
    {
        GameManager.Instance.RemoveDog(gameObject);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bird" && mTarget != null)
        {
            // Kill the bird
            mTarget.GetComponent<Bird>().Dead();

            mTarget = null;

            Dead();
        }
    }
}
