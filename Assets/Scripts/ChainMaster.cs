using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMaster : MonoBehaviour
{
    private static ChainMaster _instance;

    public GameObject moleculeToInstantiate;
    public GameObject instantiatedMolecule;

    public int counter = 0;
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
        // Find the object with the specified name in the list
        GameObject foundObject = carbonGameObjects.Find(obj => obj.name == objectName);

        return foundObject;
    }

    public void SpawnMolecule(GameObject fixedMolecule, string tagToSearch, string tagOfMoleculeToAdapt, Quaternion quaternion, string parentName)
    {
        for (var i = fixedMolecule.transform.childCount - 1; i >= 0; i--)
        {
            if (fixedMolecule.transform.GetChild(i).gameObject.CompareTag(tagToSearch))
            {
                float xLength = fixedMolecule.transform.GetChild(i + 1).gameObject.GetComponent<Renderer>().bounds.size.x;
                float yLength = fixedMolecule.transform.GetChild(i + 1).gameObject.GetComponent<Renderer>().bounds.size.y;
                Vector3 childPosition = changeDirectionToMoveMolecule(fixedMolecule.transform.GetChild(i).gameObject.transform.position, xLength, yLength, tagToSearch);

                Transform childTransform = fixedMolecule.transform.GetChild(i).gameObject.transform;
                Destroy(fixedMolecule.transform.GetChild(i).gameObject);

                instantiatedMolecule = (GameObject)Instantiate(moleculeToInstantiate, childPosition, childTransform.rotation * quaternion);
                instantiatedMolecule.name = "Carbon" + ChainMaster.Instance.counter;
                ChainMaster.Instance.counter++;
                ChainMaster.Instance.carbons.Add(new Carbon());
                ChainMaster.Instance.carbonGameObjects.Add(instantiatedMolecule);
                adaptInstantiatedMolecule(instantiatedMolecule, tagOfMoleculeToAdapt);
            }
        }
    }

    private void adaptInstantiatedMolecule(GameObject newMolecule, string tag)
    {
        for (var k = newMolecule.transform.childCount - 1; k >= 0; k--)
        {
            if (newMolecule.transform.GetChild(k).gameObject.CompareTag(tag))
            {
                Destroy(newMolecule.transform.GetChild(k).gameObject);
            }
        }
    }

    private Vector3 changeDirectionToMoveMolecule(Vector3 childPosition, float xLength, float yLength, string tagToSearch)
    {
        switch (tagToSearch)
        {
            case "Hydrogen1":
                childPosition.y += yLength;
                return childPosition;
            case "Hydrogen2":
                childPosition.x += xLength;
                return childPosition;
            case "Hydrogen3":
                childPosition.y -= yLength;
                return childPosition;
            case "Hydrogen4":
                childPosition.x -= xLength;
                return childPosition;
            default:
                return childPosition;
        }
    }
}
