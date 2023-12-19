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
    public List<NeighbourChain> attachedChainList = new List<NeighbourChain>();

    public class NeighbourChain
    {
        public int parentNode;

        public List<int> list;

        public NeighbourChain(int node, List<int> list = null)
        {
            this.parentNode = node;
            this.list = list ?? new List<int>();
        }
    }

    public class Pair<T, V, List>
    {
        // maximum distance Node
        public T first;

        // distance of maximum distance node
        public V second;

        public List<int> path;

        // Constructor
        public Pair(T first, V second, List<int> path = null)
        {
            this.first = first;
            this.second = second;
            this.path = path ?? new List<int>();
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


    public Pair<int, int, List<int>> BFS(int u)
    {
        int numOfVertices = GameMaster.Instance.numberOfVerticesInTree;
        int[] dis = new int[numOfVertices];
        int[] parent = new int[numOfVertices];

        // mark all distances with -1 
        for (int i = 0; i < numOfVertices; i++)
        {
            dis[i] = -1;
            parent[i] = -1;
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
                    parent[v] = t;
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
        int tempnodeIdx = nodeIdx;
        List<int> path = new List<int>();
        while (nodeIdx != -1)
        {
            path.Add(nodeIdx);
            nodeIdx = parent[nodeIdx];
        }
        path.Reverse();
        nodeIdx = tempnodeIdx;
        return new Pair<int, int, List<int>>(nodeIdx, maxDis, path);
    }

    public int longestPathLength()
    {
        Pair<int, int, List<int>> t1, t2;

        // first bfs to find one end point of 
        // longest path 
        t1 = BFS(0);

        // second bfs to find actual longest path 
        t2 = BFS(t1.first);
        t2.second++; //Needs +1 since it starts counting from 0

        Debug.Log("longest path is from " + t1.first +
                " to " + t2.first + " of length " + t2.second);

        Debug.Log("Nodes in the longest path:");
        foreach (var node in t2.path)
        {
            Debug.Log(node + " ");
        }

        findNeighbouringChains(t2.path);

        attachedChainList?.ForEach(temp =>
        {
            Debug.Log("Parent Node of chain: " + temp.parentNode);
            foreach (var node in temp.list)
            {
                Debug.Log(node + " ");
            }
        });

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

    public void findNeighbouringChains(List<int> longestChain)
    {
        foreach(var carbon in GameMaster.Instance.carbonGameObjects)
        {
            Carbon tempCarbonComponent = carbon.GetComponent<Carbon>();
            if(longestChain.Contains(tempCarbonComponent.numberInUndirectedTree) && tempCarbonComponent.numberOfConnectionsToMolecules > 2)
            {
                if(tempCarbonComponent.topMolecule != null && !longestChain.Contains(tempCarbonComponent.topMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    attachedChainList.Add(GetNodesNotInLongestChain(tempCarbonComponent.numberInUndirectedTree, tempCarbonComponent.topMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                        longestChain)); 
                }
                if (tempCarbonComponent.rightMolecule != null && !longestChain.Contains(tempCarbonComponent.rightMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    attachedChainList.Add(GetNodesNotInLongestChain(tempCarbonComponent.numberInUndirectedTree, tempCarbonComponent.rightMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                        longestChain));
                }
                if (tempCarbonComponent.bottomMolecule != null && !longestChain.Contains(tempCarbonComponent.bottomMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    attachedChainList.Add(GetNodesNotInLongestChain(tempCarbonComponent.numberInUndirectedTree, tempCarbonComponent.bottomMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                        longestChain));
                }
                if (tempCarbonComponent.leftMolecule != null && !longestChain.Contains(tempCarbonComponent.leftMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    attachedChainList.Add(GetNodesNotInLongestChain(tempCarbonComponent.numberInUndirectedTree, tempCarbonComponent.leftMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                        longestChain));
                }
            }
        }
    }

    public NeighbourChain GetNodesNotInLongestChain(int parentNode, int startNode, List<int> longestChain)
    {
        HashSet<int> longestChainSet = new HashSet<int>(longestChain);
        HashSet<int> visited = new HashSet<int>();
        Queue<int> queue = new Queue<int>();
        List<int> nodesNotInChain = new List<int>();

        //We add the startNode since we are sure, that it is in the neighbour chain
        nodesNotInChain.Add(startNode);

        visited.Add(startNode);
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            int currentNode = queue.Dequeue();

            // Check neighbors of the current node
            foreach (var neighbor in adjacencyList[currentNode])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);

                    // If the neighbor is not in the longest chain, add it to the result
                    if (!longestChainSet.Contains(neighbor))
                    {
                        nodesNotInChain.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return new NeighbourChain(parentNode, nodesNotInChain);
    }
}
