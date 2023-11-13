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
            GameMaster.Instance.currentWhiteboard[positionOfPlateX, positionOfPlateY] = collision.gameObject.transform.parent.gameObject;
            GameMaster.Instance.spawnablePlates.Remove(int.Parse(myCollider.name));
            Destroy(gameObject);
        }
    }

    private bool CheckIfCurrentOrientationForConnectionIsValid(GameMaster tempInstance)
    {
        if (tempInstance.counter == 0)
            return true;

        int offsetX = 0;
        int offsetY = 0;

        switch (tempInstance.currentOrientationForConnection)
        {
            case 1: offsetY = 1; break;
            case 2: offsetX = 1; break;
            case 3: offsetY = -1; break;
            case 4: offsetX = -1; break;
        }

        return !tempInstance.currentWhiteboard[positionOfPlateX + offsetX, positionOfPlateY + offsetY].gameObject.CompareTag("SpawnPlate");
    }

    public void SpawnMolecule()
    {
        GameMaster tempInstance = GameMaster.Instance;
        if(!(tempInstance.currentState == State.start))
        {
            if (tempInstance.spawnablePlates.Contains(int.Parse(gameObject.name)) && tempInstance.counter > 0 
                && (tempInstance.currentOrientationForConnection != 0 && CheckIfCurrentOrientationForConnectionIsValid(tempInstance))
                || tempInstance.spawnablePlates.Count == 0)
            {
                GameMaster.Instance.currentErrorState = ErrorState.blankBoard;
                string moleculeTag = tempInstance.SpawnNewMolecule(gameObject.transform, Quaternion.Euler(0.0f, 90f, 0.0f), positionOfPlateX, positionOfPlateY);
                Destroy(gameObject);
            }
            else
            {
                GameMaster.Instance.currentErrorState = ErrorState.illegalSpawnPlace;
                Debug.Log("Cannot place a molecule here");
            }
        }
    }

}
