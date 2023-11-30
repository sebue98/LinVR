using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private float gameObjectSizeFactor = 0.55000000000000247500000000001114f;

    public bool setBenzeneBoardActive = false;
    public int currentChoosenBenzeneCarbonForConnection = 1;
    public int currentOrientationForConnection = 0; //1 = Top, 2 = Right, 3 = Bottom, 4 = Left
    public bool currentorientationForMoleculeRing = true; //true = horizontal, false = vertical
    public GameObject carbon;
    public GameObject oxygen;
    public GameObject nitrogen;
    public GameObject sulfur;
    public GameObject benzene;
    public GameObject benzeneHorizontalOne;
    public GameObject benzeneHorizontalTwo;
    public GameObject benzeneHorizontalThree;
    public GameObject benzeneHorizontalFour;
    public GameObject benzeneHorizontalFive;
    public GameObject benzeneHorizontalSix;
    public GameObject benzeneVerticalOne;
    public GameObject benzeneVerticalTwo;
    public GameObject benzeneVerticalThree;
    public GameObject benzeneVerticalFour;
    public GameObject benzeneVerticalFive;
    public GameObject benzeneVerticalSix;

    public State currentState = State.start;
    public ErrorState currentErrorState = ErrorState.blankBoard;
    public List<int> spawnablePlates;

    public GameObject numberDecisionBoard;
    public GameObject benzeneConnectionButtonBoardHorizontal;
    public GameObject benzeneConnectionButtonBoardVertical;

    public GameObject instantiatedMolecule;

    public int counter = 0;
    public int neighbourToConnectTo = 0;
    public bool moleculeCanSpawn = false;
    public bool showDebug = false;
    public List<Carbon> carbons;
    public List<GameObject> carbonGameObjects;

    public GameObject lastMoleculeConnectedTo;
    public GameObject[,] currentWhiteboard = new GameObject[20,10];

    public IUPACAlgorithm iupac;

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
        benzeneConnectionButtonBoardHorizontal.SetActive(false);
        benzeneConnectionButtonBoardVertical.SetActive(false);
    }

    public void Update()
    {
        InputDevice handRDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputDevice handLDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        handRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool value);
        showDebug = value;
        handRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 posR);
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
        {
            //printWhiteboard();
            //Debug.Log("Longest chain found:" + iupac.FindLongestChain(currentWhiteboard));
            iupac.FindLongestChain(currentWhiteboard);
        }

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
            case "Benzene":
                {
                    switch(currentChoosenBenzeneCarbonForConnection)
                    {
                        case 1: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon1").gameObject; break;
                        case 2: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon2").gameObject; break;
                        case 3: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon3").gameObject; break;
                        case 4: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon4").gameObject; break;
                        case 5: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon5").gameObject; break;
                        case 6: currentWhiteboard[posX, posY] = instantiatedMolecule.gameObject.transform.Find("Carbon6").gameObject; break;
                    }
                    break;
                }
            case "Carbon": currentWhiteboard[posX, posY] = instantiatedMolecule; break;
        }
        currentChoosenBenzeneCarbonForConnection = 1;
    }

    public bool CheckIfBenzenePositionFitsGameBoard(int posX, int posY)
    {
        if(!currentorientationForMoleculeRing)
        {
            switch (currentChoosenBenzeneCarbonForConnection)
            {
                case 1: return (posY > 2 && (posX > 0 && posX < 19));
                case 2: return ((posY > 1 && posY < 9) && posX > 1);
                case 3: return ((posY > 0 && posY < 8) && posX > 1);
                case 4: return (posY < 7 && (posX > 0 && posX < 19));
                case 5: return ((posY > 0 && posY < 8) && posX < 18);
                case 6: return ((posY > 1 && posY < 9) && posX < 18);
            }
        }
        else
        {
            switch(currentChoosenBenzeneCarbonForConnection)
            {
                case 1: return ((posY > 0 && posY < 9) && posX < 17);
                case 2: return (posY > 2 && (posX > 0 && posX < 18));
                case 3: return (posY > 2 && (posX > 1 && posX < 19));
                case 4: return ((posY > 0 && posY < 9) && posX > 2);
                case 5: return (posY < 8 && (posX > 1 && posX < 19));
                case 6: return (posY < 9 && (posX > 0 && posX < 18));
            }
        }

        return true;
    }

    public bool SpawnNewMolecule(Transform molculeTransform, Quaternion moleculeQuaternion, int positionOfPlateX, int positionOfPlateY)
    {
        setBenzeneBoardActive = false;
        benzeneConnectionButtonBoardHorizontal.SetActive(false);
        benzeneConnectionButtonBoardVertical.SetActive(false);
        moleculeToInstantiate = SwitchSpawnMolecule();
        if (moleculeToInstantiate.CompareTag("Benzene"))// && !currentorientationForMoleculeRing)
        {
            if (!CheckIfBenzenePositionFitsGameBoard(positionOfPlateX, positionOfPlateY))
                return false;
            moleculeToInstantiate = SwitchBenzeneRingToSpawn();
        }
        instantiatedMolecule = Instantiate(moleculeToInstantiate, molculeTransform.position, moleculeQuaternion);
        instantiatedMolecule.name = currentState.ToString() + " " + counter;
        SettingMoleculesPlacesOnWhiteboard(instantiatedMolecule, positionOfPlateX, positionOfPlateY);
        CheckForNeighbourAndEstablishConnection(instantiatedMolecule, positionOfPlateX, positionOfPlateY);
        counter++;
        return true;
    }

    public void ConnectionChangerDependingOnOrientation(Carbon instantiatedCarbon, Transform connection, bool orientationChange)
    {
        switch(currentOrientationForConnection)
        {
            case 1:
                {
                    connection.position = new Vector3(instantiatedCarbon.transform.position.x, instantiatedCarbon.transform.position.y + (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.z);
                    if (orientationChange)
                        connection.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    break;
                }
            case 2:
                {
                    connection.position = new Vector3(instantiatedCarbon.transform.position.x + (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.y, instantiatedCarbon.transform.position.z);
                    if (orientationChange)
                        connection.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    break;
                }
            case 3:
                {
                    connection.position = new Vector3(instantiatedCarbon.transform.position.x, instantiatedCarbon.transform.position.y - (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.z);
                    if (orientationChange)
                        connection.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    break;
                }
            case 4:
                {
                    connection.position = new Vector3(instantiatedCarbon.transform.position.x - (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.y, instantiatedCarbon.transform.position.z);
                    if (orientationChange)
                        connection.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                    break;
                }
        }
    }

    public void SetHydrogenAndConnectionOfInstantiatedCarbon(Carbon instantiatedCarbon, string choosenHydrogen)
    {
        switch (choosenHydrogen)
        {
            case "H1":
                {
                    instantiatedCarbon.vrHydrogen1 = false;
                    Transform connection1 = instantiatedCarbon.Connection1.gameObject.transform;
                    ConnectionChangerDependingOnOrientation(instantiatedCarbon, connection1, (currentOrientationForConnection % 2 == 0));
                    break;
                }
            case "H2":
                {
                    instantiatedCarbon.vrHydrogen2 = false;
                    Transform connection2 = instantiatedCarbon.Connection2.gameObject.transform;
                    ConnectionChangerDependingOnOrientation(instantiatedCarbon, connection2, (currentOrientationForConnection % 2 != 0));
                    break;
                }
            case "H3":
                {
                    instantiatedCarbon.vrHydrogen3 = false;
                    Transform connection3 = instantiatedCarbon.Connection3.gameObject.transform;
                    ConnectionChangerDependingOnOrientation(instantiatedCarbon, connection3, (currentOrientationForConnection % 2 == 0));
                    break;
                }
            case "H4":
                {
                    instantiatedCarbon.vrHydrogen4 = false;
                    Transform conection4 = instantiatedCarbon.Connection4.gameObject.transform;
                    ConnectionChangerDependingOnOrientation(instantiatedCarbon, conection4, (currentOrientationForConnection % 2 != 0));
                    break;
                }
        }
    }

    public void CheckForNeighbourAndEstablishConnection(GameObject instantiatedMolecule, int posX, int posY)
    {
        bool benzeneInstantiated = false;
        if(instantiatedMolecule.CompareTag("Benzene"))
        {
            benzeneInstantiated = true;
            instantiatedMolecule = currentWhiteboard[posX, posY].gameObject;
        }
        
        //TODO: Check if neighbour has not the correct hydrogen or connection, to find another free hydrogen and connection to connect to
        //Check for top neighbour
        if(posY < 9 && !currentWhiteboard[posX, posY + 1].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 1)
        {
            GameObject topNeighbour = currentWhiteboard[posX, posY + 1].gameObject;
            Carbon topCarbon = topNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            topCarbon.Hydrogen3.SetActive(false);
            topCarbon.vrHydrogen3 = false;

            if (benzeneInstantiated)
            {
                string[] hydrogenAndConnection = instantiatedCarbon.GetFreeHydrogenAndConnectionForNewConnection(instantiatedCarbon.hydrogenBoolValues);
                instantiatedCarbon.transform.Find(hydrogenAndConnection[0]).gameObject.SetActive(false);
                SetHydrogenAndConnectionOfInstantiatedCarbon(instantiatedCarbon, hydrogenAndConnection[0]);
            }
            else
            {
                instantiatedCarbon.Hydrogen1.SetActive(false);
                instantiatedCarbon.vrHydrogen1 = false;
                Transform connection1 = instantiatedCarbon.Connection1.gameObject.transform;
                connection1.position = new Vector3(instantiatedCarbon.transform.position.x, instantiatedCarbon.transform.position.y + (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.z);
            }


            //Establishing connection by changing position of connections
            Transform connection3 = topCarbon.Connection3.gameObject.transform;
            connection3.position = new Vector3(topCarbon.transform.position.x, topCarbon.transform.position.y - (0.08f * gameObjectSizeFactor), topCarbon.transform.position.z);

            //Setting neighbours
            topCarbon.bottomMolecule = instantiatedMolecule;
            instantiatedCarbon.topMolecule = topNeighbour;

            //Increasing number of connections
            topCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
        }

        //Check for right neighbour
        if (posX < 19 && !currentWhiteboard[posX+1, posY].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 2)
        {
            GameObject rightNeighbour = currentWhiteboard[posX + 1, posY].gameObject;
            Carbon rightCarbon = rightNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            rightCarbon.Hydrogen4.SetActive(false);
            rightCarbon.vrHydrogen4 = false;
            
            if(benzeneInstantiated)
            {
                string[] hydrogenAndConnection = instantiatedCarbon.GetFreeHydrogenAndConnectionForNewConnection(instantiatedCarbon.hydrogenBoolValues);
                instantiatedCarbon.transform.Find(hydrogenAndConnection[0]).gameObject.SetActive(false);
                SetHydrogenAndConnectionOfInstantiatedCarbon(instantiatedCarbon, hydrogenAndConnection[0]);
            }
            else
            {
                instantiatedCarbon.Hydrogen2.SetActive(false);
                instantiatedCarbon.vrHydrogen2 = false;
                Transform connection2 = instantiatedCarbon.Connection2.gameObject.transform;
                connection2.position = new Vector3(instantiatedCarbon.transform.position.x + (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.y, instantiatedCarbon.transform.position.z);
            }
            Transform connection4 = rightCarbon.Connection4.gameObject.transform;
            connection4.position = new Vector3(rightCarbon.transform.position.x - (0.08f * gameObjectSizeFactor), rightCarbon.transform.position.y, rightCarbon.transform.position.z);

            //Setting neighbours
            rightCarbon.leftMolecule = instantiatedMolecule;
            instantiatedCarbon.rightMolecule = rightNeighbour;

            //Increasing number of connections
            rightCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
        }

        //Check for bottom neighbour
        if (posY > 0 && !currentWhiteboard[posX, posY - 1].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 3)
        {
            GameObject bottomNeighbour = currentWhiteboard[posX, posY-1].gameObject;
            Carbon bottomCarbon = bottomNeighbour.GetComponent<Carbon>();
            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            bottomCarbon.Hydrogen1.SetActive(false);
            bottomCarbon.vrHydrogen1 = false;

            if(benzeneInstantiated)
            {
                string[] hydrogenAndConnection = instantiatedCarbon.GetFreeHydrogenAndConnectionForNewConnection(instantiatedCarbon.hydrogenBoolValues);
                instantiatedCarbon.transform.Find(hydrogenAndConnection[0]).gameObject.SetActive(false);
                SetHydrogenAndConnectionOfInstantiatedCarbon(instantiatedCarbon, hydrogenAndConnection[0]);
            }
            else
            {
                instantiatedCarbon.Hydrogen3.SetActive(false);
                instantiatedCarbon.vrHydrogen3 = false;
                Transform connection3 = instantiatedCarbon.Connection3.gameObject.transform;
                connection3.position = new Vector3(instantiatedCarbon.transform.position.x, instantiatedCarbon.transform.position.y - (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.z);
            }
            Transform connection1 = bottomCarbon.Connection1.gameObject.transform;
            connection1.position = new Vector3(bottomCarbon.transform.position.x, bottomCarbon.transform.position.y + (0.08f * gameObjectSizeFactor), bottomCarbon.transform.position.z);

            //Setting neighbours
            bottomCarbon.topMolecule = instantiatedMolecule;
            instantiatedCarbon.bottomMolecule = bottomNeighbour;

            //Increasing number of connections
            bottomCarbon.numberOfConnectionsToMolecules++;
            instantiatedCarbon.numberOfConnectionsToMolecules++;
        }

        //Check for left neighbour
        if (posX > 0 && !currentWhiteboard[posX - 1, posY].gameObject.CompareTag("SpawnPlate") && currentOrientationForConnection == 4)
        {
            GameObject leftNeighbour = currentWhiteboard[posX - 1, posY].gameObject;
            Carbon leftCarbon = leftNeighbour.GetComponent<Carbon>();

            Carbon instantiatedCarbon = instantiatedMolecule.GetComponent<Carbon>();

            //Disabling Hydrogens
            leftCarbon.Hydrogen2.SetActive(false);
            leftCarbon.vrHydrogen2 = false;

            if (benzeneInstantiated)
            {
                string[] hydrogenAndConnection = instantiatedCarbon.GetFreeHydrogenAndConnectionForNewConnection(instantiatedCarbon.hydrogenBoolValues);
                instantiatedCarbon.transform.Find(hydrogenAndConnection[0]).gameObject.SetActive(false);
                SetHydrogenAndConnectionOfInstantiatedCarbon(instantiatedCarbon, hydrogenAndConnection[0]);
            }
            else
            {
                instantiatedCarbon.Hydrogen4.SetActive(false);
                instantiatedCarbon.vrHydrogen4 = false;
                Transform connection4 = instantiatedCarbon.Connection4.gameObject.transform;
                connection4.position = new Vector3(instantiatedCarbon.transform.position.x - (0.08f * gameObjectSizeFactor), instantiatedCarbon.transform.position.y, instantiatedCarbon.transform.position.z);
            }
            Transform connection2 = leftCarbon.Connection2.gameObject.transform;
            connection2.position = new Vector3(leftCarbon.transform.position.x + (0.08f * gameObjectSizeFactor), leftCarbon.transform.position.y, leftCarbon.transform.position.z);

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

    private GameObject SwitchBenzeneRingToSpawn()
    {
        if(currentorientationForMoleculeRing) //horizontal
        {
            switch(currentChoosenBenzeneCarbonForConnection)
            {
                case 1: return benzeneHorizontalOne;
                case 2: return benzeneHorizontalTwo;
                case 3: return benzeneHorizontalThree;
                case 4: return benzeneHorizontalFour;
                case 5: return benzeneHorizontalFive;
                case 6: return benzeneHorizontalSix;
            }
        }
        else //vertical
        {
            switch (currentChoosenBenzeneCarbonForConnection)
            {
                case 1: return benzeneVerticalOne;
                case 2: return benzeneVerticalTwo;
                case 3: return benzeneVerticalThree;
                case 4: return benzeneVerticalFour;
                case 5: return benzeneVerticalFive;
                case 6: return benzeneVerticalSix;
            }
        }
        return null;
    }
}
