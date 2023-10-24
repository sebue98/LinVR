using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoleculePlacer : MonoBehaviour
{
    public GameObject moleculeToPlace;
    public int positionOfPlateX;
    public int positionOfPlateY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //This code detects which plates are eligible to spawn molecules on
        Collider myCollider = collision.GetContact(0).thisCollider;
        GameMaster.Instance.spawnablePlates.Add(int.Parse(myCollider.name));
    }

    public void SpawnMolecule()
    {
        GameMaster tempInstance = GameMaster.Instance;
        if(!(tempInstance.currentState == State.start))
        {
            if ((tempInstance.spawnablePlates.Contains(int.Parse(gameObject.name)) && tempInstance.counter > 0 && tempInstance.currentOrientationForConnection != 0) || tempInstance.spawnablePlates.Count == 0)
            {
                tempInstance.SpawnNewMolecule(moleculeToPlace, gameObject.transform, Quaternion.Euler(0.0f, 90f, 0.0f), positionOfPlateX, positionOfPlateY);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Cannot place a molecule here");
            }
        }
    }

}
