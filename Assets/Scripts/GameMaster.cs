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
    benzene,
    Delete,
    DoubleConnection,
    TripleConnection,
}

public enum ErrorState
{
    illegalDoubleConnection,
    illegalSpawnPlace,
    blankBoard
}

public class GameMaster : MonoBehaviour
{
    
    private static GameMaster _instance;
    private GameObject moleculeToInstantiate;

    public int currentOrientationForConnection = 0; //1 = Top, 2 = Right, 3 = Bottom, 4 = Left
    public bool currentorientationForMoleculeRing = true;
    public GameObject carbon;
    public GameObject oxygen;
    public GameObject nitrogen;
    public GameObject sulfur;
    public GameObject benzene;
    public GameObject benzeneVertical;

    public State currentState = State.start;
    public ErrorState currentErrorState = ErrorState.blankBoard;
    public List<int> spawnablePlates;
    public GameObject numberDecisionBoard;

    public GameObject instantiatedMolecule;

    public int counter = 0;
    public int neighbourToConnectTo = 0;
    public bool moleculeCanSpawn = false;
    public bool showDebug = false;
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
        handRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool value);
        showDebug = value;
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

        if (showDebug)
            printWhiteboard();

    }

    public void printWhiteboard()
    {
        for(int y = 9; y >= 0; y--)
        {
            string currentLine = "";
            for(int x = 0; x < 20; x++)
            {
                if(currentWhiteboard[x,y] == null)
                {
                    currentLine += "X   ";
                    continue;
                }
                if(currentWhiteboard[x,y].gameObject.CompareTag("Carbon"))
                {
                    currentLine += "C   ";
                    continue;
                }
                currentLine += ".   ";
            }
            Debug.Log(currentLine);
        }
    }


    public GameObject FindObjectByName(string objectName)
    {
        GameObject foundObject = carbonGameObjects.Find(obj => obj.name == objectName);
        return foundObject;
    }

    public void SettingMoleculesPlacesOnWhiteboard(GameObject instantiatedMolecule, int posX, int posY)
    {
        switch(instantiatedMolecule.tag)
        {
            case "Benzene": currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon1").gameObject; break;
            case "Carbon": currentWhiteboard[posX, posY] = instantiatedMolecule; break;
        }
    }

    public string SpawnNewMolecule(Transform molculeTransform, Quaternion moleculeQuaternion, int positionOfPlateX, int positionOfPlateY)
    {
        moleculeToInstantiate = SwitchSpawnMolecule();
        if (moleculeToInstantiate.CompareTag("Benzene") && !currentorientationForMoleculeRing)
        {
            moleculeToInstantiate = benzeneVertical;
            moleculeQuaternion = Quaternion.Euler(90.0f, 90.0f, 0.0f);
        }
        instantiatedMolecule = Instantiate(moleculeToInstantiate, molculeTransform.position, moleculeQuaternion);
        instantiatedMolecule.name = currentState.ToString() + " " + counter;
        SettingMoleculesPlacesOnWhiteboard(instantiatedMolecule, positionOfPlateX, positionOfPlateY);
        CheckForNeighbourAndEstablishConnection(instantiatedMolecule, positionOfPlateX, positionOfPlateY);
        counter++;
        return instantiatedMolecule.tag;
    }

    public void CheckForNeighbourAndEstablishConnection(GameObject instantiatedMolecule, int posX, int posY)
    {
        if(instantiatedMolecule.CompareTag("Benzene"))
        {
            instantiatedMolecule = currentWhiteboard[posX, posY].gameObject;
        }
        
        //TODO: Check if neighbour has not the correct hydrogen or connection, to find another free hydrogen and connection to connect to
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

            //Increasing number of connections
            topCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
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

            string[] hydrogenAndConnection = instantiatedCarbon.GetFreeHydrogenAndConnectionForNewConnection(instantiatedCarbon.hydrogenBoolValues);
            instantiatedCarbon.transform.Find(hydrogenAndConnection[0]).gameObject.SetActive(false);
            switch(hydrogenAndConnection[0])
            {
                case "H1":
                    {
                        instantiatedCarbon.vrHydrogen1 = false;
                        Transform connection1 = instantiatedCarbon.Connection1.gameObject.transform;
                        connection1.position = new Vector3(0.01f, connection1.position.y + 0.01000f, connection1.position.z + 0.01000f);
                        connection1.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                        break;
                    }
                case "H2":
                    {
                        instantiatedCarbon.vrHydrogen2 = false;
                        //Transform connection2 = instantiatedCarbon.Connection2.gameObject.transform;
                        //connection2.position = new Vector3(connection2.position.x + 0.012f, connection2.position.y, connection2.position.z);
                        break;
                    }
                case "H3": instantiatedCarbon.vrHydrogen3 = false; break;
                case "H4":
                    {
                        //connection1 needs to go to position of connection2
                        instantiatedCarbon.vrHydrogen4 = false;
                        //Transform connection1 = instantiatedCarbon.Connection1.gameObject.transform;
                        //connection1.position = new Vector3(connection1.position.x, connection1.position.y + 0.012f, connection1.position.z);
                        break;
                    }
            }
            //instantiatedCarbon.transform.Find(hydrogenAndConnection[1]).gameObject.SetActive(false);
            //instantiatedCarbon.Hydrogen2.SetActive(false);
            //instantiatedCarbon.vrHydrogen2 = false;

            //Establishing connection by changing position of connections
            Transform connection4 = rightCarbon.Connection4.gameObject.transform;
            connection4.position = new Vector3(connection4.position.x - 0.012f, connection4.position.y, connection4.position.z);
            //Transform connection2 = instantiatedCarbon.Connection2.gameObject.transform;
            //connection2.position = new Vector3(connection2.position.x + 0.012f, connection2.position.y, connection2.position.z);

            //Setting neighbours
            rightCarbon.leftMolecule = instantiatedMolecule;
            instantiatedCarbon.rightMolecule = rightNeighbour;

            //Increasing number of connections
            rightCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
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

            //Increasing number of connections
            bottomCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
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

            //Increasing number of connections
            leftCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
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
            case "benzene":
                return benzene;
            default:
                return carbon;
        }
    }
}
