using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mEvilDog = null;
    [SerializeField]
    private int mStartingDogNumber = 5;
    [SerializeField]
    private float mSpawnRadius = 10.0f;

    private List<GameObject> mEvilDogList = new List<GameObject>();

    private GameObject mPlayer = null;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        // Make sure we find the player
        Debug.Assert(mPlayer != null);

        SpawnDogs(mStartingDogNumber);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to find a random position around the object
    public Vector3 FindRandomPositionAroundObject(Vector3 objectPosition)
    {
        // Generate a random direction
        Vector3 randomDirection = Random.insideUnitSphere.normalized;

        // Make sure we aren't spawning in the air or under the floor
        randomDirection.y = 0;

        // Scale the random direction by the radius
        randomDirection *= mSpawnRadius;

        // Calculate the random position around the object
        Vector3 randomPosition = objectPosition + randomDirection;

        // Return the random position
        return randomPosition;
    }

    private void SpawnDogs(int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject tmp = Instantiate(mEvilDog, transform, false);

            tmp.transform.position = FindRandomPositionAroundObject(mPlayer.transform.position);

            mEvilDogList.Add(tmp);
        }
    }
}
