using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField]
    private double size = 0.137f;
    private int count = 0;
    private double gridPositionX = -1.3f;
    private double gridPositionY = 0.53f;

    public GameObject spawnField;
    public int gridSizeX;
    public int gridSizeY;

    public void Start()
    {
        GameObject[,] whiteboard = GameMaster.Instance.currentWhiteboard;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                GameObject temp = Instantiate(spawnField, new Vector3((float)gridPositionX, (float)gridPositionY, 2.7f), Quaternion.identity);
                temp.name = count.ToString();
                temp.GetComponent<MoleculePlacer>().positionOfPlateX = x;
                temp.GetComponent<MoleculePlacer>().positionOfPlateY = y;
                whiteboard[x, y] = temp;
                gridPositionX += size;
                count++;
            }
            gridPositionY += size;
            gridPositionX = -1.3f;
        }
    }
}
