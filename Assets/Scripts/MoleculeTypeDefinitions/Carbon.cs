using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbon : MonoBehaviour
{
    public string moleculeName;

    public GameObject leftMolecule;
    public GameObject rightMolecule;
    public GameObject topMolecule;
    public GameObject bottomMolecule;

    //Properties
    public bool Hydrogen1 { get; set; }
    public bool Hydrogen2 { get; set; }
    public bool Hydrogen3 { get; set; }
    public bool Hydrogen4 { get; set; }

    public bool Connection1 { get; set; }
    public bool Connection2 { get; set; }
    public bool Connection3 { get; set; }
    public bool Connection4 { get; set; }

    public Carbon()
    {

        Hydrogen1 = true;
        Hydrogen2 = true;
        Hydrogen3 = true;
        Hydrogen4 = true;

        Connection1 = true;
        Connection2 = true;
        Connection3 = true;
        Connection4 = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //This line gives us the collided sub object of the molecule held in hand
        Collider myCollider = collision.GetContact(0).thisCollider;
        if (ChainMaster.Instance.moleculeCanSpawn)
        {
            if (myCollider.CompareTag("Hydrogen1"))
            {
                //Place new Molecule in
                topMolecule = ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen1", "Hydrogen3", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "bottom");
                //Destroy Molecule in Hand
                Destroy(collision.gameObject);
                ChainMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen2"))
            {
                rightMolecule = ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen2", "Hydrogen4", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "left");
                Destroy(collision.gameObject);
                ChainMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen3"))
            {
                bottomMolecule = ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen3", "Hydrogen1", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "top");
                Destroy(collision.gameObject);
                ChainMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen4"))
            {
                leftMolecule = ChainMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen4", "Hydrogen2", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "right");
                Destroy(collision.gameObject);
                ChainMaster.Instance.moleculeCanSpawn = false;
            }
        }
        else
        {
            GameObject HydrogenCollidedWith = collision.GetContact(0).otherCollider.gameObject;
            if (!myCollider.gameObject.CompareTag("Coal"))
            {
                if(HydrogenCollidedWith.CompareTag("Hydrogen1"))
                {
                    Debug.Log("Collided with Hydrogen1 at spawn");
                    bottomMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().topMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen2"))
                {
                    Debug.Log("Collided with Hydrogen2 at spawn");
                    leftMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().rightMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen3"))
                {
                    Debug.Log("Collided with Hydrogen3 at spawn");
                    topMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().bottomMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen4"))
                {
                    Debug.Log("Collided with Hydrogen4 at spawn");
                    rightMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().leftMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
            }
               
        }
    }

    private void SetNeighbourAndDeleteCollidingHydrogens(GameObject neighbourToSet)
    {

    }
}
