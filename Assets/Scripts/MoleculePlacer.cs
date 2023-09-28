using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoleculePlacer : MonoBehaviour
{
    public GameObject moleculeToPlace;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMolecule()
    {
        Instantiate(moleculeToPlace, gameObject.transform.position, Quaternion.Euler(0.0f, 90f, 0.0f));
        Destroy(gameObject);
    }

    public void TempFunction()
    {
        Debug.Log("Spawning molecule mode");
    }
}
