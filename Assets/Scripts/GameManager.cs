using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mEvilDog = null;
    [SerializeField]
    private int mStartingDogNumber = 5;
    [SerializeField]
    private float mSpawnRadius = 10.0f;
    [SerializeField]
    private float mScaryFaceSpeed = 50.0f;

    [SerializeField]
    private GameObject mScaryFace = null;

    private List<GameObject> mEvilDogList = new List<GameObject>();

    private List<GameObject> mBirds = new List<GameObject>();

    private GameObject mPlayer = null;

    private bool mGameOver = false;



    // Instance Stuff
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        // Make sure we find the player
        Debug.Assert(mPlayer != null, "Failed to find the player");

        SpawnDogs(mStartingDogNumber);

        // Find all the birds
        var currentBirds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (GameObject bird in currentBirds)
        {
            mBirds.Add(bird);
        }

        Debug.Assert(mBirds.Count != 0, "Failed to find the birds");
    }

    // Update is called once per frame
    void Update()
    {
        if (mEvilDogList.Count == 0 || mBirds.Count == 0)
        {
            EndSequence();
        }
    }

    private void FixedUpdate()
    {
        // Make the player look at the scary face as it rushes towards them
        if (mGameOver)
        {
            mScaryFace.transform.position = Vector3.MoveTowards(mScaryFace.transform.position, mPlayer.transform.position, mScaryFaceSpeed * Time.fixedDeltaTime);

            mPlayer.transform.LookAt(mScaryFace.transform.transform);

            if (Vector3.Distance(mScaryFace.transform.position, mPlayer.transform.position) < 5.0f)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
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

    public void RemoveDog(GameObject dogToRemove)
    {
        mEvilDogList.Remove(dogToRemove);
    }

    public void RemoveBird(GameObject birdToRemove)
    {
        mBirds.Remove(birdToRemove);
    }

    /// <summary>
    /// Lock player movement and return them to the begining of the level
    /// </summary>
    private void EndSequence()
    {
        mGameOver = true;

        mPlayer.GetComponent<PlayerMovement>().InputEnabled = false;

        mPlayer.transform.position = Vector3.zero;
    }
}
