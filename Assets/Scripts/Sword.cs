using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If the sword hits the dog, kill it
        if (other.gameObject.tag == "Dog")
        {
            other.GetComponent<EvilDog>().Dead();
        }
    }
}
