using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject carbon;
    public GameObject oxygen;
    public GameObject nitrogen;
    public GameObject sulfur;

    public State currentState = State.start;
    public List<int> spawnablePlates;
    public GameObject numberDecisionBoard;

    private GameObject moleculeToInstantiate;
    public GameObject instantiatedMolecule;

    public int counter = 0;
    public int neighbourToConnectTo = 0;
    public bool moleculeCanSpawn = false;
    public List<Carbon> carbons;
    public List<GameObject> carbonGameObjects;

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



    public GameObject FindObjectByName(string objectName)
    {
        GameObject foundObject = carbonGameObjects.Find(obj => obj.name == objectName);
        return foundObject;
    }

    public GameObject SpawnNewMolecule(GameObject fixedMolecule, Transform molculeTransform, Quaternion moleculeQuaternion)
    {
        moleculeToInstantiate = SwitchSpawnMolecule();
        Instantiate(moleculeToInstantiate, molculeTransform.position, moleculeQuaternion);
        return instantiatedMolecule;
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
