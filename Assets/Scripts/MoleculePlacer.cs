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
        ChainMaster.Instance.SpawnNewMolecule(moleculeToPlace, gameObject.transform, Quaternion.Euler(0.0f, 90f, 0.0f));
        //Instantiate(moleculeToPlace, gameObject.transform.position, Quaternion.Euler(0.0f, 90f, 0.0f));
        Destroy(gameObject);
    }

    public void TempFunction()
    {
        Debug.Log("Spawning molecule mode");
    }

    public void SetCarbon()
    {
        ChainMaster.Instance.currentState = State.carbon;
    }

    public void SetHydrogen()
    {
        ChainMaster.Instance.currentState = State.hydrogen;
    }

    public void SetNitrogen()
    {
        ChainMaster.Instance.currentState = State.nitrogen;
    }

    public void SetSulfur()
    {
        ChainMaster.Instance.currentState = State.sulfur;
    }
}
