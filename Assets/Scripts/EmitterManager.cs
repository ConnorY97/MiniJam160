using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterManager : MonoBehaviour
{
    private static EmitterManager instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
