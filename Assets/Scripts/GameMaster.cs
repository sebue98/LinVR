using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum State
{
    start,
    carbon,
    oxygen,
    nitrogen,
    sulfur,
    Delete,
    DoubleConnection,
    TripleConnection,
}

public class GameMaster : MonoBehaviour
{
    
    private static GameMaster _instance;
    private GameObject moleculeToInstantiate;

    public int currentOrientationForConnection = 0; //1 = Top, 2 = Right, 3 = Bottom, 4 = Left
    public GameObject carbon;
    public GameObject oxygen;
    public GameObject nitrogen;
    public GameObject sulfur;

    public State currentState = State.start;
    public List<int> spawnablePlates;
    public GameObject numberDecisionBoard;

    public GameObject instantiatedMolecule;

    public int counter = 0;
    public int neighbourToConnectTo = 0;
    public bool moleculeCanSpawn = false;
    public List<Carbon> carbons;
    public List<GameObject> carbonGameObjects;

    public GameObject lastMoleculeConnectedTo;
    public GameObject[,] currentWhiteboard = new GameObject[20,10];

    public static GameMaster Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            currentState = State.start;
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy this instance if another one already exists
            Destroy(gameObject);
        }
        numberDecisionBoard.SetActive(false);
    }

    public void Update()
    {
        InputDevice handRDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputDevice handLDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        handLDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 posR);
        //Top Condition
        if(posR.y > 0.5f  && (- 0.5f < posR.x && posR.x < 0.5f))
        {
            //Debug.Log("Top");
            currentOrientationForConnection = 1;
        }
        else if(posR.x > 0.5f && (-0.5f < posR.y && posR.y < 0.5f))
        {
            //Debug.Log("Right");
            currentOrientationForConnection = 2;
        }
        else if(posR.y < -0.5f && (-0.5f < posR.x && posR.x < 0.5f))
        {
            //Debug.Log("Bottom");
            currentOrientationForConnection = 3;
        }
        else if(posR.x < -0.5f && (-0.5f < posR.y && posR.y < 0.5f))
        {
            //Debug.Log("Left");
            currentOrientationForConnection = 4;
        }
    }



    public GameObject FindObjectByName(string objectName)
    {
        GameObject foundObject = carbonGameObjects.Find(obj => obj.name == objectName);
        return foundObject;
    }

    public GameObject SpawnNewMolecule(GameObject fixedMolecule, Transform molculeTransform, Quaternion moleculeQuaternion, int positionOfPlateX, int positionOfPlateY)
    {
        moleculeToInstantiate = SwitchSpawnMolecule();
        instantiatedMolecule = Instantiate(moleculeToInstantiate, molculeTransform.position, moleculeQuaternion);
        instantiatedMolecule.name = currentState.ToString() + " " + counter;
        currentWhiteboard[positionOfPlateX, positionOfPlateY] = instantiatedMolecule;
        CheckForNeighbourAndEstablishConnection(instantiatedMolecule, positionOfPlateX, positionOfPlateY);
        counter++;
        return instantiatedMolecule;
    }

    public void CheckForNeighbourAndEstablishConnection(GameObject instantiatedMolecule, int posX, int posY)
    {
        //Check for top neighbour
        if(!currentWhiteboard[posX, posY + 1].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 1)
        {
            GameObject topNeighbour = currentWhiteboard[posX, posY + 1].gameObject;
            Carbon topCarbon = topNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            topCarbon.Hydrogen3.SetActive(false);
            topCarbon.vrHydrogen3 = false;
            instantiatedCarbon.Hydrogen1.SetActive(false);
            instantiatedCarbon.vrHydrogen1 = false;

            //Establishing connection by changing position of connections
            Transform connection3 = topCarbon.Connection3.gameObject.transform;
            connection3.position = new Vector3(connection3.position.x, connection3.position.y - 0.012f, connection3.position.z);
            Transform connection1 = instantiatedCarbon.Connection1.gameObject.transform;
            connection1.position = new Vector3(connection1.position.x, connection1.position.y + 0.012f, connection1.position.z);

            //Setting neighbours
            topCarbon.bottomMolecule = instantiatedMolecule;
            instantiatedCarbon.topMolecule = topNeighbour;
            //Debug.Log("Top Neighbour is existing");
        }

        //Check for right neighbour
        if (!currentWhiteboard[posX+1, posY].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 2)
        {
            GameObject rightNeighbour = currentWhiteboard[posX + 1, posY].gameObject;
            Carbon rightCarbon = rightNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            rightCarbon.Hydrogen4.SetActive(false);
            rightCarbon.vrHydrogen4 = false;
            instantiatedCarbon.Hydrogen2.SetActive(false);
            instantiatedCarbon.vrHydrogen2 = false;

            //Establishing connection by changing position of connections
            Transform connection4 = rightCarbon.Connection4.gameObject.transform;
            connection4.position = new Vector3(connection4.position.x - 0.012f, connection4.position.y, connection4.position.z);
            Transform connection2 = instantiatedCarbon.Connection2.gameObject.transform;
            connection2.position = new Vector3(connection2.position.x + 0.012f, connection2.position.y, connection2.position.z);

            //Setting neighbours
            rightCarbon.leftMolecule = instantiatedMolecule;
            instantiatedCarbon.rightMolecule = rightNeighbour;
            //Debug.Log("Right Neighbour is existing");
        }

        //Check for bottom neighbour
        if (!currentWhiteboard[posX, posY - 1].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 3)
        {
            GameObject bottomNeighbour = currentWhiteboard[posX, posY-1].gameObject;
            Carbon bottomCarbon = bottomNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            bottomCarbon.Hydrogen1.SetActive(false);
            bottomCarbon.vrHydrogen1 = false;
            instantiatedCarbon.Hydrogen3.SetActive(false);
            instantiatedCarbon.vrHydrogen3 = false;

            //Establishing connection by changing position of connections
            Transform connection3 = instantiatedCarbon.Connection3.gameObject.transform;
            connection3.position = new Vector3(connection3.position.x, connection3.position.y - 0.012f, connection3.position.z);
            Transform connection1 = bottomCarbon.Connection1.gameObject.transform;
            connection1.position = new Vector3(connection1.position.x, connection1.position.y + 0.012f, connection1.position.z);

            //Setting neighbours
            bottomCarbon.topMolecule = instantiatedMolecule;
            instantiatedCarbon.bottomMolecule = bottomNeighbour;
            //Debug.Log("Bottom Neighbour is existing");
        }

        //Check for left neighbour
        if (!currentWhiteboard[posX - 1, posY].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 4)
        {
            GameObject leftNeighbour = currentWhiteboard[posX - 1, posY].gameObject;
            Carbon leftCarbon = leftNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            leftCarbon.Hydrogen2.SetActive(false);
            leftCarbon.vrHydrogen2 = false;
            instantiatedCarbon.Hydrogen4.SetActive(false);
            instantiatedCarbon.vrHydrogen4 = false;

            //Establishing connection by changing position of connections
            Transform connection4 = instantiatedCarbon.Connection4.gameObject.transform;
            connection4.position = new Vector3(connection4.position.x - 0.012f, connection4.position.y, connection4.position.z);
            Transform connection2 = leftCarbon.Connection2.gameObject.transform;
            connection2.position = new Vector3(connection2.position.x + 0.012f, connection2.position.y, connection2.position.z);

            //Setting neighbours
            leftCarbon.rightMolecule = instantiatedMolecule;
            instantiatedCarbon.leftMolecule = leftNeighbour;
            //Debug.Log("Left Neighbour is existing");
        }

    }

    private GameObject SwitchSpawnMolecule()
    {
        switch (currentState.ToString())
        {
            case "carbon":
                return carbon;
            case "oxygen":
                return oxygen;
            case "nitrogen":
                return nitrogen;
            case "sulfur":
                return sulfur;
            default:
                return carbon;
        }
    }

    /*
    public GameObject SpawnMolecule(GameObject fixedMolecule, string tagToSearch, string tagOfMoleculeToAdapt, Quaternion quaternion, string parentName, string neighbourSide)
    {
        Debug.Log(currentState.ToString());
        moleculeToInstantiate = SwitchSpawnMolecule();
        for (var i = fixedMolecule.transform.childCount - 1; i >= 0; i--)
        {
            if (fixedMolecule.transform.GetChild(i).gameObject.CompareTag(tagToSearch))
            {
                float xLength = fixedMolecule.transform.GetChild(i + 1).gameObject.GetComponent<Renderer>().bounds.size.x;
                float yLength = fixedMolecule.transform.GetChild(i + 1).gameObject.GetComponent<Renderer>().bounds.size.y;
                //Vector3 childPosition = ChangeDirectionToMoveMolecule(fixedMolecule.transform.GetChild(i).gameObject, xLength, yLength, tagToSearch);

                Vector3 offset = ChangeDirectionToMoveMolecule(fixedMolecule.transform.GetChild(i).gameObject, xLength, yLength, tagToSearch);


                Transform childTransform = fixedMolecule.transform.GetChild(i).gameObject.transform;
                Destroy(fixedMolecule.transform.GetChild(i).gameObject);

                instantiatedMolecule = (GameObject)Instantiate(moleculeToInstantiate, fixedMolecule.transform.GetChild(i).gameObject.transform.position + offset, childTransform.rotation * quaternion);
                SetNeighbour(fixedMolecule, instantiatedMolecule, neighbourSide);
                instantiatedMolecule.name = "Carbon" + GameMaster.Instance.counter;
                GameMaster.Instance.counter++;
                GameMaster.Instance.carbons.Add(new Carbon());
                GameMaster.Instance.carbonGameObjects.Add(instantiatedMolecule);
                AdaptInstantiatedMolecule(instantiatedMolecule, tagOfMoleculeToAdapt);
            }
        }
        return instantiatedMolecule;
    }

    
    private void AdaptInstantiatedMolecule(GameObject newMolecule, string tag)
    {
        for (var k = newMolecule.transform.childCount - 1; k >= 0; k--)
        {
            if (newMolecule.transform.GetChild(k).gameObject.CompareTag(tag))
            {
                Destroy(newMolecule.transform.GetChild(k).gameObject);
            }
        }
    }

    private Vector3 ChangeDirectionToMoveMolecule(GameObject childPosition, float xLength, float yLength, string tagToSearch)
    {
        switch (tagToSearch)
        {
            case "Hydrogen1":
                    return childPosition.transform.up * yLength;
            case "Hydrogen2":
                    return childPosition.transform.forward * xLength;
            case "Hydrogen3":
                return -childPosition.transform.up * yLength;
            case "Hydrogen4":
                return -childPosition.transform.forward * xLength;
            default:
            return childPosition.transform.forward;
        }
    }

    private void SetNeighbour(GameObject fixedMolecule, GameObject instantiatedMolecule, string neighbourSide)
    {
        switch(neighbourSide)
        {
            case "top":
                instantiatedMolecule.GetComponent<Carbon>().topMolecule = fixedMolecule;
                break;
            case "left":
                instantiatedMolecule.GetComponent<Carbon>().leftMolecule = fixedMolecule;
                break;
            case "bottom":
                instantiatedMolecule.GetComponent<Carbon>().bottomMolecule = fixedMolecule;
                break;
            case "right":
                instantiatedMolecule.GetComponent<Carbon>().rightMolecule = fixedMolecule;
                break;
        }
    }
    */
}
