using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndirectedGraph
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<int, List<int>> adjacencyList;

    public class Pair<T, V>
    {
        // maximum distance Node
        public T first;

        // distance of maximum distance node
        public V second;

        // Constructor
        public Pair(T first, V second)
        {
            this.first = first;
            this.second = second;
        }
    }

    public UndirectedGraph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    // Function to add a new vertex to the graph
    public void AddVertex(int vertex)
    {
        if (!adjacencyList.ContainsKey(vertex))
        {
            adjacencyList[vertex] = new List<int>();
        }
    }

    // Function to add an edge between two vertices
    public void AddEdge(int source, int destination)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            AddVertex(source);
        }
        if (!adjacencyList.ContainsKey(destination))
        {
            AddVertex(destination);
        }

        adjacencyList[source].Add(destination);
        adjacencyList[destination].Add(source); // For an undirected graph
    }


    public Pair<int, int> BFS(int u)
    {
        int numOfVertices = GameMaster.Instance.numberOfVerticesInTree;
        int[] dis = new int[numOfVertices];

        // mark all distances with -1 
        for (int i = 0; i < numOfVertices; i++)
        {
            dis[i] = -1;
        }

        Queue<int> q = new Queue<int>();

        q.Enqueue(u);

        // distance of u from u will be 0 
        dis[u] = 0;

        while (q.Count != 0)
        {
            int t = q.Dequeue();

            // loop for all adjacent nodes of node-t 
            foreach (var v in adjacencyList[t])
            {
                // push node into queue only if 
                // it is not visited already 
                if (dis[v] == -1)
                {
                    q.Enqueue(v);

                    // make distance of v, one more 
                    // than distance of t 
                    dis[v] = dis[t] + 1;
                }
            }
        }

        int maxDis = 0;
        int nodeIdx = 0;

        // get farthest node distance and its index 
        for (int i = 0; i < numOfVertices; ++i)
        {
            if (dis[i] > maxDis)
            {
                maxDis = dis[i];
                nodeIdx = i;
            }
        }

        return new Pair<int, int>(nodeIdx, maxDis);
    }

    public int longestPathLength()
    {
        Pair<int, int> t1, t2;

        // first bfs to find one end point of 
        // longest path 
        t1 = BFS(0);

        // second bfs to find actual longest path 
        t2 = BFS(t1.first);
        t2.second++; //Needs +1 since it starts counting from 0

        Debug.Log("longest path is from " + t1.first +
                " to " + t2.first + " of length " + t2.second);

        return t2.second;
    }

    // Function to print the adjacency list representation of the graph
    public void PrintGraph()
    {
        foreach (var kvp in adjacencyList)
        {
            Debug.Log ($"Vertex {kvp.Key}: ");
            foreach (var neighbor in kvp.Value)
            {
                Debug.Log($"{neighbor} ");
            }
            Debug.Log(" ");
        }
    }
}
