using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class IUPACAlgorithm : MonoBehaviour
{
    public static List<GameObject> longestChainElements = new List<GameObject>();
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
        Debug.Log("Current Longest Chain: " + longestChain.Count);
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

    /*
    public int FindLongestChain(GameObject[,] grid)
    {
        int maxChainLength = 0;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j].gameObject.CompareTag("Carbon"))
                {
                    List<GameObject> tempChain = new List<GameObject>();
                    int length = DFS(grid, i, j, rows, cols, tempChain);
                    Debug.Log("Current length: " + length + "Current tempchain length: " + tempChain.Count);
                    //maxChainLength = Math.Max(maxChainLength, length);
                    if (length > maxChainLength)
                    {
                        maxChainLength = length;
                        Debug.Log("Temp Chain Length: " + tempChain.Count); // Debug output
                        longestChain.Clear();
                        longestChain.AddRange(tempChain);
                        Debug.Log("Longest Chain Lenth: " + longestChain.Count);
                    }
                }
            }
        }

        return maxChainLength;
    }

    public int DFS(GameObject[,] grid, int i, int j, int rows, int cols, List<GameObject> currentChain)
    {
        if (i < 0 || i >= rows || j < 0 || j >= cols || !grid[i, j].gameObject.CompareTag("Carbon"))
        {
            return 0;
        }

        grid[i, j].gameObject.tag = "Visited";
        currentChain.Add(grid[i, j].gameObject);
        Debug.Log("Current State of currentChain in DFS function: " + currentChain.Count);

        int maxLength = 0;

        for (int k = 0; k < directions.GetLength(0); k++)
        {
            int newX = i + directions[k, 0];
            int newY = j + directions[k, 1];

            // Check if the new coordinates are within bounds
            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
            {
                int length = DFS(grid, newX, newY, rows, cols, currentChain);
                maxLength = Math.Max(maxLength, length);
            }
        }

        grid[i, j].gameObject.tag = "Carbon"; // Restore the cell to its original state
        currentChain.RemoveAt(currentChain.Count - 1);

        return maxLength + 1;
    }*/
}