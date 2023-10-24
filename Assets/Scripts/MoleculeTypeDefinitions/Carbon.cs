using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Carbon : MonoBehaviour
{
    public string moleculeName;

    public GameObject leftMolecule;
    public GameObject rightMolecule;
    public GameObject topMolecule;
    public GameObject bottomMolecule;

    public GameObject Hydrogen1;
    public GameObject Hydrogen2;
    public GameObject Hydrogen3;
    public GameObject Hydrogen4;

    public GameObject Connection1;
    public GameObject Connection2;
    public GameObject Connection3;
    public GameObject Connection4;

    //Properties
    public bool vrHydrogen1;
    public bool vrHydrogen2;
    public bool vrHydrogen3;
    public bool vrHydrogen4;

    public bool vrConnection1;
    public bool vrConnection2;
    public bool vrConnection3;
    public bool vrConnection4;

    public bool[] hydrogenBoolValues = new bool[4];
    public bool[] hydrogenConnectionValues = new bool[4];

    public int numberOfConnectedMolecules = 0;

    public Carbon()
    {

        vrHydrogen1 = true;
        vrHydrogen2 = true;
        vrHydrogen3 = true;
        vrHydrogen4 = true;

        vrConnection1 = true;
        vrConnection2 = true;
        vrConnection3 = true;
        vrConnection4 = true;
    }

    public void Update()
    {
        hydrogenBoolValues[0] = vrHydrogen1;
        hydrogenBoolValues[1] = vrHydrogen2;
        hydrogenBoolValues[2] = vrHydrogen3;
        hydrogenBoolValues[3] = vrHydrogen4;

        hydrogenConnectionValues[0] = vrConnection1;
        hydrogenConnectionValues[1] = vrConnection2;
        hydrogenConnectionValues[2] = vrConnection3;
        hydrogenConnectionValues[3] = vrConnection4;
    }

    public string[] GetHydrogenToDeleteForDoubleConnection(bool[] hydrogenBoolArray)
    {
        string hydrogenToDeleteForDoubleConnection = "";
        string connectionToChangeForDoubleConnection = "";
        int count = 1;
        for (; count <= hydrogenBoolArray.Length; count++)
        {
            if (hydrogenBoolArray[count - 1])
            {
                hydrogenToDeleteForDoubleConnection = "H" + count;
                connectionToChangeForDoubleConnection = "Connection" + count;
                break;
            }
        }
        hydrogenBoolArray[count-1] = false;
        

        string[] result = { hydrogenToDeleteForDoubleConnection, connectionToChangeForDoubleConnection};

        return result;
    }

    public void CreateDoubleConnection()
    {
        if(GameMaster.Instance.neighbourToConnectTo > 0 && numberOfConnectedMolecules < 4)
        {
            switch(GameMaster.Instance.neighbourToConnectTo)
            {
                case 1:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection2", "Connection1", "Connection3", topMolecule);
                    break;
                case 2:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection1", "Connection2", "Connection4", rightMolecule);
                    break;
                case 3:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection2", "Connection3", "Connection1", bottomMolecule);
                    break;
                case 4:
                    SetMoleculeAndHydrogenForDoubleConnection("Connection1", "Connection4", "Connection2", leftMolecule);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetMoleculeAndHydrogenForDoubleConnection(string connection1, string choosenMoleculeConnection, string moleculeToConnectConnection, GameObject moleculeToconnectTo)
    {
        GameMaster.Instance.numberDecisionBoard.SetActive(false);
        Carbon carbonComponentOfMoleculeToConnect = moleculeToconnectTo.GetComponent<Carbon>();
        string[] choosenMoleculeAttributeArray = GetHydrogenToDeleteForDoubleConnection(hydrogenBoolValues);
        string[] moleculeToConnectAttributeArray = GetHydrogenToDeleteForDoubleConnection(carbonComponentOfMoleculeToConnect.hydrogenBoolValues);

        //Increase connected molecule's counter of both gameobjects
        numberOfConnectedMolecules++;
        carbonComponentOfMoleculeToConnect.numberOfConnectedMolecules++;

        //Choosen molecule for double connection
        GameObject MOC1 = gameObject.transform.Find(choosenMoleculeAttributeArray[1]).gameObject;
        GameObject MOC2 = gameObject.transform.Find(choosenMoleculeConnection).gameObject;

        //Disable choosen molecule's hydrogen gameobject
        gameObject.transform.Find(choosenMoleculeAttributeArray[0]).gameObject.SetActive(false);

        //Neighbouring molecule for double connection
        GameObject MTC1 = moleculeToconnectTo.transform.Find(moleculeToConnectAttributeArray[1]).gameObject;
        GameObject MTC2 = moleculeToconnectTo.transform.Find(moleculeToConnectConnection).gameObject;

        //Disable choosen molecule-to-connect-to's hydrogen gameobject
        moleculeToconnectTo.transform.Find(moleculeToConnectAttributeArray[0]).gameObject.SetActive(false);

        //horizontal = true, vertical = false
        if(GameMaster.Instance.neighbourToConnectTo % 2 == 0)
        {
            //Changing the position of the connections of the choosen molecule
            MOC1.transform.position = new Vector3(MOC2.transform.position.x, MOC2.transform.position.y + 0.01f, MOC2.transform.position.z);
            MOC1.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            MOC2.transform.position = new Vector3(MOC2.transform.position.x, MOC2.transform.position.y - 0.01f, MOC2.transform.position.z);

            //Changing the positon of the connections of the neighbouring molecule
            MTC1.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y + 0.01f, MTC2.transform.position.z);
            MTC1.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            MTC2.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y - 0.01f, MTC2.transform.position.z);
        }
        else
        {

            //Changing the position of the connections of the choosen molecule
            MOC1.transform.position = new Vector3(MOC2.transform.position.x + 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);
            MOC1.transform.rotation = MOC2.transform.rotation;
            MOC2.transform.position = new Vector3(MOC2.transform.position.x - 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);

            //Changing the positon of the connections of the neighbourign molecule
            MTC1.transform.position = new Vector3(MTC2.transform.position.x + 0.01f, MTC2.transform.position.y, MTC2.transform.position.z);
            MTC1.transform.rotation = MTC2.transform.rotation;
            MTC2.transform.position = new Vector3(MTC2.transform.position.x - 0.01f, MTC2.transform.position.y , MTC2.transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
            //This line gives us the collided sub object of the molecule collided with
            Collider myCollider = collision.GetContact(0).thisCollider;
            GameObject HydrogenCollidedWith = collision.GetContact(0).otherCollider.gameObject;
            if (!myCollider.gameObject.CompareTag("Coal") || !myCollider.gameObject.CompareTag("SpawnPlate"))
            {
                Debug.Log(myCollider.gameObject.name);
                Debug.Log(HydrogenCollidedWith.gameObject.name);

                if(HydrogenCollidedWith.CompareTag("Hydrogen1"))
                {
                    numberOfConnectedMolecules++;
                    vrHydrogen3 = false;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().vrHydrogen1 = false;
                    bottomMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    bottomMolecule.GetComponent<Carbon>().vrHydrogen1 = false;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().topMolecule = this.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.SetActive(false);
                    myCollider.gameObject.SetActive(false);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen2"))
                {
                    numberOfConnectedMolecules++;
                    vrHydrogen4 = false;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().vrHydrogen2 = false;
                    leftMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().rightMolecule = this.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.SetActive(false);
                    myCollider.gameObject.SetActive(false);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen3"))
                {
                    numberOfConnectedMolecules++;
                    vrHydrogen1 = false;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().vrHydrogen3 = false;
                    topMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().bottomMolecule = this.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.SetActive(false);
                    myCollider.gameObject.SetActive(false);
                }
                else if(HydrogenCollidedWith.CompareTag("Hydrogen4"))
                {
                    numberOfConnectedMolecules++;
                    vrHydrogen2 = false;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().vrHydrogen4 = false;
                    rightMolecule = collision.GetContact(0).otherCollider.gameObject.transform.parent.gameObject;
                    HydrogenCollidedWith.transform.parent.gameObject.GetComponent<Carbon>().leftMolecule = this.gameObject;
                    collision.GetContact(0).otherCollider.gameObject.SetActive(false);
                    myCollider.gameObject.SetActive(false);
                }
            }*/
    }
}
