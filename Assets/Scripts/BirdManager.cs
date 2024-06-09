using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] private Transform mTarget;
    private List<Bird> mBirds;

    private void Start()
    {
        mBirds = GameManager.Instance.birds;

        if (mTarget != null)
        {
            foreach (var bird in mBirds)
            {
                bird.target = mTarget;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var birdA in mBirds)
        {
            int numPerceivedBirds = 0;
            int numBirdsToAvoid = 0;

            Vector3 centerOfFlock = Vector3.zero;
            birdA.flockAlignment = Vector3.zero;
            birdA.flockSeparation = Vector3.zero;

            foreach (var birdB in mBirds)
            {
                if (birdA == birdB)
                    continue;

                Vector3 offset = birdB.transform.position - birdA.transform.position;
                float distance = offset.magnitude;

                if (distance <= birdA.perceptionRadius)
                {
                    numPerceivedBirds++;
                    centerOfFlock += birdB.transform.position;
                    birdA.flockAlignment += birdB.velocity;

                    if (distance <= birdA.separationRadius)
                    {
                        numBirdsToAvoid++;

                        float inverseFraction = 1.0f - (distance / birdA.separationRadius);
                        birdA.flockSeparation -= offset * inverseFraction;
                    }
                }
            }

            if (numPerceivedBirds > 0)
            {
                centerOfFlock /= numPerceivedBirds;
                birdA.offsetToCenterOfFlock = centerOfFlock - birdA.transform.position;
                birdA.flockAlignment /= numPerceivedBirds;
            }

            if (numBirdsToAvoid > 0)
                birdA.flockSeparation /= numBirdsToAvoid;

            birdA.UpdateBoid();
        }
    }
}