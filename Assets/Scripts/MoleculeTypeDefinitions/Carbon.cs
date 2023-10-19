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

    public int numberOfConnectedMolecules = 0;

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

    public void Update()
    {

    }

    public void CreateDoubleConnection()
    {
        if(GameMaster.Instance.neighbourToConnectTo > 0)
        {
            switch(GameMaster.Instance.neighbourToConnectTo)
            {
                
                //Case C1,C2,C4,C1
                case 2:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection1", "Connection2", "Connection4", "H1", rightMolecule);
                    break;
                case 3:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection2", "Connection3", "Connection1", "H2", bottomMolecule);
                    break;
                //Case C1,C4,C1,C2
                case 4:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection1", "Connection4", "Connection2", "H1", leftMolecule);
                    break;
                default:
                    break;
            }


        }
    }

    public void SetMoleculeAndHydrogenForDoubleConnection(string connection1, string connection2, string connection3, string hydrogenToHide, GameObject moleculeToconnectTo)
    {
        GameMaster.Instance.numberDecisionBoard.SetActive(false);
        //Case C1,C2,C4,C1
        GameObject MOC1 = gameObject.transform.Find(connection1).gameObject;
        GameObject MOC2 = gameObject.transform.Find(connection2).gameObject;

        gameObject.transform.Find(hydrogenToHide).gameObject.SetActive(false);

        GameObject MTC1 = moleculeToconnectTo.transform.Find(connection1).gameObject;
        GameObject MTC2 = moleculeToconnectTo.transform.Find(connection3).gameObject;

        moleculeToconnectTo.transform.Find(hydrogenToHide).gameObject.SetActive(false);

        //horizontal = true, vertical = false
        if(GameMaster.Instance.neighbourToConnectTo % 2 == 0)
        {
            //Changing the position of the top and the right connection rod
            MOC1.transform.position = new Vector3(MOC2.transform.position.x, MOC2.transform.position.y + 0.01f, MOC2.transform.position.z);
            MOC1.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            MOC2.transform.position = new Vector3(MOC2.transform.position.x, MOC2.transform.position.y - 0.01f, MOC2.transform.position.z);

            //Changing the positon of the top and the left connection rod
            MTC1.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y + 0.01f, MTC2.transform.position.z);
            MTC1.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            MTC2.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y - 0.01f, MTC2.transform.position.z);
        }
        else
        {

            //Changing the position of the top and the right connection rod
            MOC1.transform.position = new Vector3(MOC2.transform.position.x + 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);
            MOC1.transform.rotation = MOC2.transform.rotation;
            MOC2.transform.position = new Vector3(MOC2.transform.position.x - 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);

            //Changing the positon of the top and the left connection rod
            MTC1.transform.position = new Vector3(MTC2.transform.position.x + 0.01f, MTC2.transform.position.y, MTC2.transform.position.z);
            MTC1.transform.rotation = MTC2.transform.rotation;
            MTC2.transform.position = new Vector3(MTC2.transform.position.x - 0.01f, MTC2.transform.position.y , MTC2.transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //This line gives us the collided sub object of the molecule held in hand
        Collider myCollider = collision.GetContact(0).thisCollider;
        if (GameMaster.Instance.moleculeCanSpawn)
        {
            if (myCollider.CompareTag("Hydrogen1"))
            {
                //Place new Molecule in
                topMolecule = GameMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen1", "Hydrogen3", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "bottom");
                //Destroy Molecule in Hand
                Destroy(collision.gameObject);
                GameMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen2"))
            {
                rightMolecule = GameMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen2", "Hydrogen4", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "left");
                Destroy(collision.gameObject);
                GameMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen3"))
            {
                bottomMolecule = GameMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen3", "Hydrogen1", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "top");
                Destroy(collision.gameObject);
                GameMaster.Instance.moleculeCanSpawn = false;
            }
            else if (myCollider.CompareTag("Hydrogen4"))
            {
                leftMolecule = GameMaster.Instance.SpawnMolecule(this.gameObject, "Hydrogen4", "Hydrogen2", Quaternion.Euler(0f, 0f, 0f), myCollider.transform.parent.name, "right");
                Destroy(collision.gameObject);
                GameMaster.Instance.moleculeCanSpawn = false;
            }
        }
        else
        {
            GameObject HydrogenCollidedWith = collision.GetContact(0).otherCollider.gameObject;
            if (!myCollider.gameObject.CompareTag("Coal"))
            {
                if(HydrogenCollidedWith.CompareTag("Hydrogen1"))
                {
                    //Debug.Log("Collided with Hydrogen1 at spawn");
                    bottomMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().topMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen2"))
                {
                    //Debug.Log("Collided with Hydrogen2 at spawn");
                    leftMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().rightMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen3"))
                {
                   // Debug.Log("Collided with Hydrogen3 at spawn");
                    topMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().bottomMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen4"))
                {
                    //Debug.Log("Collided with Hydrogen4 at spawn");
                    rightMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject.GetComponent<Carbon>().leftMolecule = this.gameObject;
                    Destroy(collision.GetContact(0).otherCollider.gameObject);
                    Destroy(myCollider.gameObject);
                }
            }
               
        }
    }
}
