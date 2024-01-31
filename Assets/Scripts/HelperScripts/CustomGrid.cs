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
    public GameObject carbon;
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

    public void ShowTaskSolutionOnGameBoard(List<(int, int)> solutionList)
    {
        gridSizeY = 10;
        gridSizeX = 20;
        GameObject[,] whiteboard = GameMaster.Instance.currentWhiteboard;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Destroy(whiteboard[x, y].gameObject);
            }
        }

        count = 0;
        gridPositionX = -1.3f;
        gridPositionY = 0.53f;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if(solutionList.Contains((x,y)))
                {
                    gridPositionX += size;
                    count++;
                }
                else
                {
                    GameObject temp = Instantiate(spawnField, new Vector3((float)gridPositionX, (float)gridPositionY, 2.7f), Quaternion.identity);
                    temp.name = count.ToString();
                    temp.GetComponent<MoleculePlacer>().positionOfPlateX = x;
                    temp.GetComponent<MoleculePlacer>().positionOfPlateY = y;
                    whiteboard[x, y] = temp;
                    gridPositionX += size;
                    count++;
                }
            }
            gridPositionY += size;
            gridPositionX = -1.3f;
        }

        foreach(var carbon in GameMaster.Instance.carbonGameObjects)
        {
            GameMaster.Instance.CheckForNeighbourAndEstablishConnection(carbon, carbon.GetComponent<Carbon>().positionXOnWhiteboard, carbon.GetComponent<Carbon>().positionYOnWhiteboard, false);
        }
    }

    public void RebuildGameBoard()
    {
        gridSizeY = 10;
        gridSizeX = 20;
        GameObject[,] whiteboard = GameMaster.Instance.currentWhiteboard;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Destroy(whiteboard[x, y].gameObject);
            }
        }
        count = 0;
        gridPositionX = -1.3f;
        gridPositionY = 0.53f;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
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

