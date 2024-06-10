using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArea : MonoBehaviour
{
    private PlayerController mPlayer = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            mPlayer = other.GetComponent<PlayerController>();
            mPlayer.InLight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mPlayer != null)
        {
            mPlayer.InLight = false;
            mPlayer = null;
        }
    }
}
