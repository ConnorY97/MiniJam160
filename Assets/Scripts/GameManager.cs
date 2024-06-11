using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool mDebugging = false;
    [SerializeField]
    private GameObject mScaryFace = null;
    public GameObject ScaryFace
    {
        get { return mScaryFace; }
    }

    private List<GameObject> mEvilDogs = new List<GameObject>();

    public List<Bird> birds => mBirds;
    private List<Bird> mBirds;

    [SerializeField]
    private GameObject mPlayer = null;
    private PlayerController mPlayerReference = null;

    private bool mGameOver = false;
    public bool GameOver
    {
        get { return mGameOver; }
    }

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

        // Make sure we find the player
        Debug.Assert(mPlayer != null, "Failed to find the player");

        mPlayerReference = mPlayer.GetComponent<PlayerController>();

        Debug.Assert(mPlayerReference != null, "Failed to assign player reference from the mPlayer");

        SpawnDogs(mStartingDogNumber);

        // Find all the birds
        mBirds = FindObjectsOfType<Bird>().ToList();

        Debug.Assert(mBirds.Count != 0, "Failed to find the birds");

        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure the game isn't over
        if (!mGameOver)
        {
            // Note (Rey): Why do we lose if all dogs are defeated?
            // Fixing that now
            // Lose if you run out of health or you run out of birds
            if (mPlayerReference.Health <= 0 || mBirds.Count <= 0)
            {
                EndSequence();
            }
            // Spawn more dogs if you still have birds but no more dogs
            if (mEvilDogs.Count == 0 && mBirds.Count != 0)
            {
                SpawnDogs(mStartingDogNumber);
            }
        }

        if (mDebugging)
        {
            if (mDebugging)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    EndSequence();
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    SpawnDogs(mStartingDogNumber);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    KillAllDogs();
                }
            }
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

            mEvilDogs.Add(tmp);
        }
    }

    public void RemoveDog(GameObject dogToRemove)
    {
        mEvilDogs.Remove(dogToRemove);
    }

    public void RemoveBird(Bird birdToRemove)
    {
        mBirds.Remove(birdToRemove);
    }

    /// <summary>
    /// Lock player movement and return them to the begining of the level
    /// </summary>
    private void EndSequence()
    {
        mGameOver = true;

        mPlayerReference.InputEnabled = false;

        mPlayer.transform.position = Vector3.zero;

        // Remove reaming birds and dogs
        KillAllBirds();
        KillAllDogs();
    }

    private void KillAllDogs()
    {
        foreach (var dog in mEvilDogs)
        {
            dog.gameObject.SetActive(false);
            Destroy(dog);
        }
        mEvilDogs.Clear();
    }

    private void KillAllBirds()
    {
        foreach (var bird in mBirds)
        {
            bird.gameObject.SetActive(false);
            Destroy(bird);
        }
        mBirds.Clear();
    }
}
