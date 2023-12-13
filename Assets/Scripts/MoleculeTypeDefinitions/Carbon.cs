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

    public int positionXOnWhiteboard;
    public int positionYOnWhiteboard;
    public int numberInUndirectedTree;

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

    public int numberOfConnectionsToMolecules = 0;

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

    public string[] GetFreeHydrogenAndConnectionForNewConnection(bool[] hydrogenBoolArray)
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
                switch(count-1)
                {
                    case 0: vrHydrogen1 = false;
                        break;
                    case 1: vrHydrogen2 = false;
                        break;
                    case 2: vrHydrogen3 = false;
                        break;
                    case 3: vrHydrogen4 = false;
                        break;
                }
                break;
            }
        }
       
        

        string[] result = { hydrogenToDeleteForDoubleConnection, connectionToChangeForDoubleConnection};

        return result;
    }

    public void CreateDoubleConnection()
    {
        GameMaster GMInstance = GameMaster.Instance;
        if(GMInstance.neighbourToConnectTo > 0 && numberOfConnectionsToMolecules < 4)
        {
            switch(GMInstance.neighbourToConnectTo)
            {
                case 1:
                    if (topMolecule == null)
                    {
                        GMInstance.currentErrorState = ErrorState.illegalDoubleConnection;
                        break;
                    }
                    else
                        SetMoleculeAndHydrogenForDoubleConnection("Connection1", "Connection3", topMolecule);
                       break;
                case 2:
                    if (rightMolecule == null)
                    {
                        GMInstance.currentErrorState = ErrorState.illegalDoubleConnection;
                        break;
                    }
                    else
                        SetMoleculeAndHydrogenForDoubleConnection("Connection2", "Connection4", rightMolecule);
                       break;
                case 3:
                    if (bottomMolecule == null)
                    {
                        GMInstance.currentErrorState = ErrorState.illegalDoubleConnection;
                        break;
                    }
                    else
                        SetMoleculeAndHydrogenForDoubleConnection("Connection3", "Connection1", bottomMolecule);
                    break;
                case 4:
                    if (leftMolecule == null)
                    {
                        GMInstance.currentErrorState = ErrorState.illegalDoubleConnection;
                        break;
                    }
                    else
                        SetMoleculeAndHydrogenForDoubleConnection("Connection4", "Connection2", leftMolecule);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetMoleculeAndHydrogenForDoubleConnection(string choosenMoleculeConnection, string moleculeToConnectConnection, GameObject moleculeToconnectTo)
    {
        GameMaster.Instance.currentErrorState = ErrorState.blankBoard;
        Debug.Log("Setting double connection");
        GameMaster.Instance.numberDecisionBoard.SetActive(false);
        Carbon carbonComponentOfMoleculeToConnect = moleculeToconnectTo.GetComponent<Carbon>();
        if (carbonComponentOfMoleculeToConnect.numberOfConnectionsToMolecules > 3)
        {
            GameMaster.Instance.currentErrorState = ErrorState.illegalDoubleConnection;
            return;
        }
        GameMaster.Instance.currentErrorState = ErrorState.blankBoard;
        string[] choosenMoleculeAttributeArray = GetFreeHydrogenAndConnectionForNewConnection(hydrogenBoolValues);
        string[] moleculeToConnectAttributeArray = carbonComponentOfMoleculeToConnect.GetFreeHydrogenAndConnectionForNewConnection(carbonComponentOfMoleculeToConnect.hydrogenBoolValues);

        //Increase connected molecule's counter of both gameobjects
        numberOfConnectionsToMolecules++;
        carbonComponentOfMoleculeToConnect.numberOfConnectionsToMolecules++;

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
            if (MOC1.CompareTag("Connection1") || MOC1.CompareTag("Connection3"))
                MOC1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            //For else we do not need a rotation since all of them have 0,0,0
            MOC2.transform.position = new Vector3(MOC2.transform.position.x, MOC2.transform.position.y - 0.01f, MOC2.transform.position.z);

            //Changing the positon of the connections of the neighbouring molecule
            MTC1.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y + 0.01f, MTC2.transform.position.z);
            if (MTC1.CompareTag("Connection1") || MTC1.CompareTag("Connection3"))
                MTC1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            MTC2.transform.position = new Vector3(MTC2.transform.position.x, MTC2.transform.position.y - 0.01f, MTC2.transform.position.z);
        }
        else
        {

            //Changing the position of the connections of the choosen molecule
            MOC1.transform.position = new Vector3(MOC2.transform.position.x + 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);
            if (MOC1.CompareTag("Connection2") || MOC2.CompareTag("Connection4"))
                MOC1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            MOC2.transform.position = new Vector3(MOC2.transform.position.x - 0.01f, MOC2.transform.position.y, MOC2.transform.position.z);

            //Changing the positon of the connections of the neighbouring molecule
            MTC1.transform.position = new Vector3(MTC2.transform.position.x + 0.01f, MTC2.transform.position.y, MTC2.transform.position.z);
            if (MTC1.CompareTag("Connection2") || MTC1.CompareTag("Connection4"))
                MTC1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            MTC2.transform.position = new Vector3(MTC2.transform.position.x - 0.01f, MTC2.transform.position.y , MTC2.transform.position.z);
        }
    }
}
