using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Spawn,
    carbon,
    hydrogen,
    nitrogen,
    sulfur,
    Delete,
}

public class ChainMaster : MonoBehaviour
{
    
    private static ChainMaster _instance;

    public GameObject carbon;
    public GameObject hydrogen;
    public GameObject nitrogen;
    public GameObject sulfur;

    public State currentState = State.Spawn;

    private GameObject moleculeToInstantiate;
    public GameObject instantiatedMolecule;

    public int counter = 0;
    public bool moleculeCanSpawn = false;
    public List<Carbon> carbons;
    public List<GameObject> carbonGameObjects;

    public static ChainMaster Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            currentState = State.Spawn;
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy this instance if another one already exists
            Destroy(gameObject);
        }
    }



    public GameObject FindObjectByName(string objectName)
    {
        GameObject foundObject = carbonGameObjects.Find(obj => obj.name == objectName);
        return foundObject;
    }

    public GameObject SpawnNewMolecule(GameObject fixedMolecule, Transform molculeTransform, Quaternion moleculeQuaternion)
    {
        Debug.Log(currentState.ToString());
        moleculeToInstantiate = SwitchSpawnMolecule();
        Instantiate(moleculeToInstantiate, molculeTransform.position, moleculeQuaternion);
        return instantiatedMolecule;
    }

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
                instantiatedMolecule.name = "Carbon" + ChainMaster.Instance.counter;
                ChainMaster.Instance.counter++;
                ChainMaster.Instance.carbons.Add(new Carbon());
                ChainMaster.Instance.carbonGameObjects.Add(instantiatedMolecule);
                AdaptInstantiatedMolecule(instantiatedMolecule, tagOfMoleculeToAdapt);
            }
        }
        return instantiatedMolecule;
    }

    private GameObject SwitchSpawnMolecule()
    {
        switch(currentState.ToString())
        {
            case "carbon":
                return carbon;
            case "hydrogen":
                return hydrogen;
            case "nitrogen":
                return nitrogen;
            case "sulfur":
                return sulfur;
            default:
                return carbon;
        }
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
}
