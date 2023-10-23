using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField]
    private float size = 0.137f;
    private int count = 0;
    private float gridPositionX = -1.3f;
    private float gridPositionY = 0.53f;

    public GameObject spawnField;
    public int gridSizeX;
    public int gridSizeY;

    public void Start()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                GameObject temp = Instantiate(spawnField, new Vector3(gridPositionX, gridPositionY, 2.7f), Quaternion.identity);
                temp.name = count.ToString();
                gridPositionX += size;
                count++;
            }
            gridPositionY += size;
            gridPositionX = -1.3f;
        }
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
