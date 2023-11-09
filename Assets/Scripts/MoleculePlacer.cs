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
        //For future context: even though carbon object and spawnplate do NOT collide, it still detects a collision
        //because of the "collision offset" predefined in unity. We will keep this offset due to convencience of the
        //objects placed and we still need this kind of collision to be detected for setting the spawnable plates.

        //This code detects which plates are eligible to spawn molecules on
        Collider myCollider = collision.GetContact(0).thisCollider;
        if (!GameMaster.Instance.spawnablePlates.Contains(int.Parse(myCollider.name)))
        {
            GameMaster.Instance.spawnablePlates.Add(int.Parse(myCollider.name));
        }
        
        if (collision.gameObject.CompareTag("Coal"))
        {
            Debug.Log("Carbon collision detected with " + collision.gameObject.transform.parent.name);
            GameMaster.Instance.currentWhiteboard[positionOfPlateX, positionOfPlateY] = collision.gameObject.transform.parent.gameObject;
            GameMaster.Instance.spawnablePlates.Remove(int.Parse(myCollider.name));
            Destroy(gameObject);
        }
    }

    public void SpawnMolecule()
    {
        GameMaster tempInstance = GameMaster.Instance;
        if(!(tempInstance.currentState == State.start))
        {
            if ((tempInstance.spawnablePlates.Contains(int.Parse(gameObject.name)) && tempInstance.counter > 0 && tempInstance.currentOrientationForConnection != 0) || tempInstance.spawnablePlates.Count == 0)
            {
                string moleculeTag = tempInstance.SpawnNewMolecule(gameObject.transform, Quaternion.Euler(0.0f, 90f, 0.0f), positionOfPlateX, positionOfPlateY);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Cannot place a molecule here");
            }
        }
    }

}
