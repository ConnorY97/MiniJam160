using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderLight : MonoBehaviour
{
    [SerializeField]
    private float mSpeed = 7.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * mSpeed * Time.deltaTime);
    }
}
