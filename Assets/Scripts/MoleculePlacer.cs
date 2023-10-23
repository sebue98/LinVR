using System;
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

    private void OnCollisionEnter(Collision collision)
    {
        //This code detects which plates are eligible to spawn molecules on
        Collider myCollider = collision.GetContact(0).thisCollider;
        GameMaster.Instance.spawnablePlates.Add(int.Parse(myCollider.name));
    }

    public void SpawnMolecule()
    {
        if(!(GameMaster.Instance.currentState == State.start))
        {
            if (GameMaster.Instance.spawnablePlates.Contains(int.Parse(gameObject.name)) || GameMaster.Instance.spawnablePlates.Count == 0)
            {
                GameMaster.Instance.SpawnNewMolecule(moleculeToPlace, gameObject.transform, Quaternion.Euler(0.0f, 90f, 0.0f));
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Cannot place a molecule here");
            }
        }
    }

}
