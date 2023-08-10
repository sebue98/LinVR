using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrogenCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //This line gives us the collided sub object of the molecule held in hand
        Collider myCollider = collision.GetContact(0).thisCollider;

        /*
        //This line checks with which of the 4 molecules we have collided with
        Collider collider = collision.collider;
        if (collider.CompareTag("Hydrogen1"))
        {
            Debug.Log(gameObject.tag + " Collided with Hydrogen 1");
        }
        else if (collider.CompareTag("Hydrogen2"))
        {
            Debug.Log(gameObject.tag + " Collided with Hydrogen 2");
        }
        else if (collider.CompareTag("Hydrogen3"))
        {
            Debug.Log(gameObject.tag + " Collided with Hydrogen 3");
        }
        else if (collider.CompareTag("Hydrogen4"))
        {
            Debug.Log(gameObject.tag + " Collided with Hydrogen 4");
        }

        Debug.Log(gameObject.name + " collided with: " + collision.gameObject.name);*/
    }
}

