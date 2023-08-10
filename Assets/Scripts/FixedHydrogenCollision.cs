using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedHydrogenCollision : MonoBehaviour
{
    public GameObject moleculeToInstantiate;

    private void OnCollisionEnter(Collision collision)
    {
        //This line gives us the collided sub object of the molecule held in hand
        Collider myCollider = collision.GetContact(0).thisCollider;
        //Need to delete the right hydrogen when clipping onto the existing hydrogen

        //This line checks with which of the 4 molecules we have collided with

        //This collider refers to the object that was held in hand and collided with the other molecule
        Collider collider = collision.collider;
        Debug.Log("We collided with: " + collider.tag);
        if (collider.CompareTag("Hydrogen1"))
        {
            //searchForHydrogenToDelete("Hydrogen1", Quaternion.Euler(90f, 90f, -90f));
        }
        else if (collider.CompareTag("Hydrogen2"))
        {
            //searchForHydrogenToDelete("Hydrogen1", Quaternion.Euler(90f, 90f, -90f));
        }
        else if (collider.CompareTag("Hydrogen3"))
        {
            //Debug.Log(gameObject.tag + " Collided with Hydrogen 3");
        }
        else if (collider.CompareTag("Hydrogen4"))
        {
            //Debug.Log(gameObject.tag + " Collided with Hydrogen 4");
        }

        //Debug.Log(gameObject.name + " collided with: " + collision.gameObject.name);
    }

    private void searchForHydrogenToDelete(string tagToSearch, Quaternion quaternion)
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (gameObject.transform.GetChild(i).gameObject.CompareTag(tagToSearch))
            {
                Vector3 childPosition = gameObject.transform.GetChild(i).gameObject.transform.position;
                Transform childTransform = gameObject.transform.GetChild(i).gameObject.transform;
                Destroy(gameObject.transform.GetChild(i).gameObject);
                GameObject tempMolecule = Instantiate(moleculeToInstantiate, childPosition, childTransform.rotation * quaternion);
                adaptInstantiatedMolecule(tempMolecule, tagToSearch);
            }
        }
    }

    private void adaptInstantiatedMolecule(GameObject newMolecule, string tag)
    {
        for (var k = newMolecule.transform.childCount - 1; k >= 0; k--)
        {
            if (moleculeToInstantiate.transform.GetChild(k).gameObject.CompareTag(tag))
            {
                Destroy(newMolecule.transform.GetChild(k).gameObject);
                Destroy(newMolecule.transform.GetChild(k + 1).gameObject);
            }
        }
    }
}

