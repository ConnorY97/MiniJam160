using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Slider mHealthBar = null;
    [SerializeField]
    private float mHealth = 100.0f;
    public float Health
    {
        get { return mHealth; }
    }
    [SerializeField]
    private float mDamage = 5.0f;
    [SerializeField]
    private float mDamageDelay = 0.25f;
    [SerializeField]
    private bool mInLight = false;
    public bool InLight
    {
        set { mInLight = value; }
    }

    [SerializeField]
    private Animator mAnimator = null;

    private float mDamageTime = 0.0f;

    [SerializeField]
    private Volume mVolume = null;
    private Vignette mVignette = null;

    private bool mInputEnabled = true;
    public bool InputEnabled
    {
        get { return mInputEnabled; }
        set { mInputEnabled = value; }
    }

    void Start()
    {
        mDamageTime = Time.time + mDamageDelay;
        mVolume.profile.TryGet(out Vignette vignette);
        mVignette = vignette;
    }

    void Update()
    {
        // Outside of the light take damage
        if (mInLight == false && mDamageTime < Time.time)
        {
            mHealth -= mDamage;
            mDamageTime = Time.time + mDamageDelay;
        }
        // Inside of the light heal
        else if (mInLight == true && mDamageTime < Time.time)
        {
            if (mHealth < 100.0f)
            {
                mHealth += mDamage;
            }
            mDamageTime = Time.time + mDamageDelay;
        }

        mHealthBar.value = mHealth;

        // Animations
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mAnimator.SetTrigger("Attack");
        }

        // UI
        if (mInLight)
        {
            mVignette.intensity.value = 0.0f;
        }
        else
        {
            mVignette.intensity.value = 0.4f;
        }
    }
}