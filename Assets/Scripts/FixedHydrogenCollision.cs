using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedHydrogenCollision : MonoBehaviour
{
    public bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        //This line gives us the collided sub object of the molecule held in hand
        Collider myCollider = collision.GetContact(0).thisCollider;

        if (myCollider.CompareTag("Hydrogen1"))
        {
            //Place new Molecule in
            ChainMaster.Instance.SpawnMolecule(this.gameObject,"Hydrogen1", "Hydrogen3", Quaternion.Euler(90f, 90f, 90f), myCollider.transform.parent.name);
            //Destroy Molecule in Hand
            Destroy(collision.gameObject);
        }
        else if (myCollider.CompareTag("Hydrogen2"))
        {
            ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen2", "Hydrogen4", Quaternion.Euler(90f, -180f, 180f), myCollider.transform.parent.name);
            Destroy(collision.gameObject);
        }
        else if (myCollider.CompareTag("Hydrogen3"))
        {
            ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen3", "Hydrogen1", Quaternion.Euler(90f, 90f, 90f), myCollider.transform.parent.name);
            Destroy(collision.gameObject);
        }
        else if (myCollider.CompareTag("Hydrogen4"))
        {
            ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen4", "Hydrogen2", Quaternion.Euler(90f, -180f, 180f), myCollider.transform.parent.name);
            Destroy(collision.gameObject);
        }

    }
}

