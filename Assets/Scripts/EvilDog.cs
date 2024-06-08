using UnityEngine;

public class EvilDog : MonoBehaviour
{
    private GameObject mTarget = null;

    [SerializeField]
    private float mSpeed = 8.0f;

    [SerializeField]
    public float mAttackRange = 5.0f;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, mTarget.transform.position, mSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, mTarget.transform.position) < mAttackRange)
        {
            // Kill the bird
            Destroy(mTarget);

            // Then kill itself
            Destroy(gameObject);
        }
    }
}
