using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


public class IUPACAlgorithm : MonoBehaviour
{
    public List<GameObject> longestChainElements = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

    public List<GameObject> longestChainGlobal = new List<GameObject>();
    public List<List<GameObject>> neighboringChainsElements = new List<List<GameObject>>();

    public String CreateIUPACName(int lengthOfChain, int typeOfConnectionSDT)
    {
        string[] startMolecules = { "Methan", "Ethan", "Propan", "Butan", "Pentan", "Hexan", "Heptan", "Octan", "Nonan", "Decan", "Undecan", "Duodecan",
        "Tridecan", "Tetradecan", "Pentadecan", "Hexadecan", "Heptadecan", "Octadecan", "Nonadecan", "Eicosan", "Heneicosan"};
        string[] prefixes = {"", "Hen", "Do", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona" }; //Will only be used for molecules with size > 21
        string[] suffixes = { "n", "en", "in" }; //single connection, double connection, triple connection
        string[] middlePart = {"", "deca", "cosa", "triaconta", "tetraconta", "pentaconta", "hexaconta", "heptaconta", "octaconta", "nonaconta"};

        if(lengthOfChain < 22)
        {
            return startMolecules[lengthOfChain - 1];
        }
        else if(lengthOfChain >= 22 && lengthOfChain < 100)
        {
            int tensPlace = (lengthOfChain / 10) % 10;
            int onesPlace = lengthOfChain % 10;

            string tempString = "";
            tempString += prefixes[onesPlace];
            tempString += middlePart[tensPlace];

            return tempString + suffixes[typeOfConnectionSDT];
        }
        else
        {
            int tensPlace = (lengthOfChain / 10) % 10;
            int onesPlace = lengthOfChain % 10;

            string tempString = "";
            tempString += prefixes[onesPlace];
            tempString += middlePart[tensPlace];

            return tempString + "hecta" + suffixes[typeOfConnectionSDT];
        }
    }

    public  List<GameObject> FindLongestChain(GameObject[,] grid)
    {
        int maxChainLength = 0;
        List<GameObject> longestChain = new List<GameObject>();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j].gameObject.CompareTag("Carbon"))
                {
                    List<GameObject> currentPath = DFS(grid, i, j, rows, cols);

                    if (currentPath.Count > maxChainLength)
                    {
                        maxChainLength = currentPath.Count;
                        longestChain = currentPath;
                    }
                }
            }
        }
        //Debug.Log("Current Longest Chain: " + longestChain.Count);
        longestChainGlobal = longestChain;
        return longestChain;
    }

    public List<GameObject> DFS(GameObject[,] grid, int i, int j, int rows, int cols)
    {
        if (i < 0 || i >= rows || j < 0 || j >= cols || !grid[i, j].gameObject.CompareTag("Carbon"))
        {
            return new List<GameObject>();
        }

        grid[i, j].gameObject.tag = "Visited";

        List<List<GameObject>> neighbors = new List<List<GameObject>>();

        for (int k = 0; k < directions.GetLength(0); k++)
        {
            int newX = i + directions[k,0];
            int newY = j + directions[k,1];

            List<GameObject> path = DFS(grid, newX, newY, rows, cols);
            neighbors.Add(path);
        }

        grid[i, j].gameObject.tag = "Carbon";

        var longestNeighbor = neighbors.OrderByDescending(n => n.Count).FirstOrDefault();

        List<GameObject> currentPath = new List<GameObject>();
        currentPath.Add(grid[i,j].gameObject);
        currentPath.AddRange(longestNeighbor);

        return currentPath;
    }

    public List<GameObject> GetConnectedChain(GameObject[,] grid, int startX, int startY, int neighborX, int neighborY, HashSet<(int, int)> visited)
    {
        List<GameObject> connectedChain = new List<GameObject>();
        int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

        // Determine the direction between start and neighbor coordinates
        int dirX = neighborX - startX;
        int dirY = neighborY - startY;

        int dirIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (directions[i, 0] == dirX && directions[i, 1] == dirY)
            {
                dirIndex = i;
                break;
            }
        }

        if (dirIndex != -1)
        {
            int newX = startX + directions[dirIndex, 0];
            int newY = startY + directions[dirIndex, 1];

            if (newX >= 0 && newX < 20 && newY >= 0 && newY < 10 && grid[newX, newY].gameObject.CompareTag("Carbon") && !visited.Contains((newX, newY)))
            {
                visited.Add((newX, newY));
                connectedChain.Add(grid[newX, newY].gameObject);

                //connectedChain.AddRange(GetConnectedChain(grid, newX, newY, startX, startY, visited));
                List<GameObject> recursiveChain = GetConnectedChain(grid, newX, newY, startX, startY, visited);
                connectedChain.AddRange(recursiveChain);
            }
        }
        if(connectedChain.Count != 0)
            GameMaster.Instance.tempNeighbourObjects.Add(connectedChain);
        return connectedChain;
    }

}