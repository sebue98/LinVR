using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrid : MonoBehaviour
{
    [SerializeField]
    private float size = 0.1f;
    public GameObject spawnField;
    public int gridSizeX;
    public int gridSizeY;

    private int count = 0;
    private float gridPositionX = -1f;
    private float gridPositionY = 0.5f;

    public void Start()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                GameObject temp = Instantiate(spawnField, new Vector3(gridPositionX, gridPositionY, 1f), Quaternion.identity);
                temp.name = "Spawnfield " + count;
                gridPositionX += size;
                count++;
            }
            gridPositionY += size;
            gridPositionX = -1f;
        }
    }

    public void Update()
    {
        //Debug.Log(_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue)); 
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3((float)xCount * size, (float)yCount * size, (float)zCount * size);

        result += transform.position;

        return result;
    }
}
