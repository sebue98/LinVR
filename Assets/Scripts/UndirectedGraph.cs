using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UndirectedGraph
{
    public Dictionary<int, List<int>> adjacencyList;
    public List<NeighbourChain> attachedChainList = new List<NeighbourChain>();

    //Orignal n-ary tree with longest path:
    ///////////////////////////////////////////
    
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
        return t2.second;
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 


    public class NeighbourChain
    {
        public int parentNode;

        public Dictionary<int, List<int>> list;

        public NeighbourChain(int node, Dictionary<int, List<int>> list = null)
        {
            this.parentNode = node;
            this.list = list ?? new Dictionary<int, List<int>>();
        }
    }



    public void AddVertexSubtree(int vertex, Dictionary<int, List<int>> subTree)
    {
        if(!subTree.ContainsKey(vertex))
        {
            subTree[vertex] = new List<int>();
        }
    }

    public void AddEdgeSubtree(int source, int destination, Dictionary<int, List<int>> subTree)
    {
        if (!subTree.ContainsKey(source))
        {
            AddVertexSubtree(source, subTree);
        }
        if (!subTree.ContainsKey(destination))
        {
            AddVertexSubtree(destination, subTree);
        }

        subTree[source].Add(destination);
        subTree[destination].Add(source); // For an undirected graph
    }


    public Pair<int, int, List<int>> BFS2(int u, Dictionary<int, List<int>> adjList)
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
            foreach (var v in adjList[t])
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

    public IUPACNameStructureElement findPathLengthsForIUPACName()
    {
        //List<CorrespondingNodePair> orderedLongestPathList = new List<CorrespondingNodePair>(); //Used to name the longest path from 1-n
        List<int> namingList = new List<int>(); //Used for the IUPAC name
        Pair<int, int, List<int>> t1, t2;

        //First run to find longest path
        //t1 = BFS(0);
        t1 = BFS(adjacencyList.ElementAt(0).Key);
        t2 = BFS(t1.first);
        t2.second++;
        namingList.Add(t2.second);
        t2.path.Reverse(); //Makes the list so it is from left to right numbered

        //Check neighbouring trees of longest path
        List<NeighbourChain> tempList = new List<NeighbourChain>();
        findNeighbouringSubtrees(t2.path, tempList);

        List<LengthAndListAndParentNodePair> lengthAndNodePairList = new List<LengthAndListAndParentNodePair>();
        foreach(var pair in tempList)
        {
            if(pair.list.Count == 1)
            {
                lengthAndNodePairList.Add(new LengthAndListAndParentNodePair(1, pair.parentNode, new List<int>(1) { pair.list.ElementAt(0).Key }));
            }
            else if(pair.list.Count > 1)
            {
                Pair<int, int, List<int>> f1, f2;
                f1 = BFS2(pair.list.ElementAt(0).Key, pair.list);
                f2 = BFS2(f1.first, pair.list);
                f2.path.Reverse();
                f2.second++;
                lengthAndNodePairList.Add(new LengthAndListAndParentNodePair(f2.second, pair.parentNode, f2.path));
            }
        }

        return new IUPACNameStructureElement(t2.second, lengthAndNodePairList, t2.path);
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

    public void findNeighbouringSubtrees(List<int> longestChain, List<NeighbourChain> tempList)
    {
        foreach (var carbon in GameMaster.Instance.carbonGameObjects)
        {
            Carbon tempCarbonComponent = carbon.GetComponent<Carbon>();
            if (longestChain.Contains(tempCarbonComponent.numberInUndirectedTree) && tempCarbonComponent.numberOfConnectionsToMolecules > 2)
            {
                if (tempCarbonComponent.topMolecule != null && !longestChain.Contains(tempCarbonComponent.topMolecule.GetComponent<Carbon>().numberInUndirectedTree) &&
                    !GameMaster.Instance.nodeNumbersOfBenzeneRings.Contains(tempCarbonComponent.topMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    tempList.Add(new NeighbourChain(tempCarbonComponent.numberInUndirectedTree, GetSubtreeOfNonLongestChainNeighborsNew(tempCarbonComponent.topMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                         longestChain)));
                }
                if (tempCarbonComponent.rightMolecule != null && !longestChain.Contains(tempCarbonComponent.rightMolecule.GetComponent<Carbon>().numberInUndirectedTree) &&
                    !GameMaster.Instance.nodeNumbersOfBenzeneRings.Contains(tempCarbonComponent.rightMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    tempList.Add(new NeighbourChain(tempCarbonComponent.numberInUndirectedTree, GetSubtreeOfNonLongestChainNeighborsNew(tempCarbonComponent.rightMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                         longestChain)));
                }
                if (tempCarbonComponent.bottomMolecule != null && !longestChain.Contains(tempCarbonComponent.bottomMolecule.GetComponent<Carbon>().numberInUndirectedTree) &&
                    !GameMaster.Instance.nodeNumbersOfBenzeneRings.Contains(tempCarbonComponent.bottomMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    tempList.Add(new NeighbourChain(tempCarbonComponent.numberInUndirectedTree, GetSubtreeOfNonLongestChainNeighborsNew(tempCarbonComponent.bottomMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                         longestChain)));
                }
                if (tempCarbonComponent.leftMolecule != null && !longestChain.Contains(tempCarbonComponent.leftMolecule.GetComponent<Carbon>().numberInUndirectedTree) &&
                    !GameMaster.Instance.nodeNumbersOfBenzeneRings.Contains(tempCarbonComponent.leftMolecule.GetComponent<Carbon>().numberInUndirectedTree))
                {
                    tempList.Add(new NeighbourChain(tempCarbonComponent.numberInUndirectedTree, GetSubtreeOfNonLongestChainNeighborsNew(tempCarbonComponent.leftMolecule.GetComponent<Carbon>().numberInUndirectedTree,
                         longestChain)));
                }
            }
        }
    }

    public Dictionary<int, List<int>> GetSubtreeOfNonLongestChainNeighborsNew(int nodeFromLongestChain, List<int> longestChain)
    {
        HashSet<int> longestChainSet = new HashSet<int>(longestChain);
        Dictionary<int, List<int>> subtree = new Dictionary<int, List<int>>();

        Queue<int> queue = new Queue<int>();
        HashSet<int> visited = new HashSet<int>();

        visited.Add(nodeFromLongestChain);
        queue.Enqueue(nodeFromLongestChain);

        // Add the root node to the subtree
        AddVertexSubtree(nodeFromLongestChain, subtree);

        while (queue.Count > 0)
        {
            int currentNode = queue.Dequeue();
            //List<int> currentNeighbors = new List<int>();

            // Check neighbors of the current node
            foreach (var neighbor in adjacencyList[currentNode])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);

                    // If the neighbor is not part of the longest chain, add it to the subtree
                    if (!longestChainSet.Contains(neighbor))
                    {
                        //currentNeighbors.Add(neighbor);
                        queue.Enqueue(neighbor);

                        // Add the vertex to the subtree
                        AddVertexSubtree(neighbor, subtree);

                        // Add the edge to the subtree
                        AddEdgeSubtree(currentNode, neighbor, subtree);
                    }
                }
            }

            //subtree[currentNode] = currentNeighbors;
        }

        return subtree;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////
    ///Naming for CycloAlkanes
    ///

    public IUPACNameStructureElement findNamingForCycloAlkanes()
    {
        List<int> nodeNumbersOfCycloAlkanes = GameMaster.Instance.nodeNumbersOfBenzeneRings;
        List<NeighbourChain> chainsOfCycloAlkanes = new List<NeighbourChain>();
        List<LengthAndListAndParentNodePair> tempList = new List<LengthAndListAndParentNodePair>();
        findNeighbouringSubtrees(nodeNumbersOfCycloAlkanes, chainsOfCycloAlkanes);

        List<LengthAndListAndParentNodePair> lengthAndNodePairList = new List<LengthAndListAndParentNodePair>();
        foreach (var pair in chainsOfCycloAlkanes)
        {
            if (pair.list.Count == 1)
            {
                lengthAndNodePairList.Add(new LengthAndListAndParentNodePair(1, pair.parentNode, new List<int>(1) { pair.list.ElementAt(0).Key }));
            }
            else if (pair.list.Count > 1)
            {
                Pair<int, int, List<int>> f1, f2;
                f1 = BFS2(pair.list.ElementAt(0).Key, pair.list);
                f2 = BFS2(f1.first, pair.list);
                f2.path.Reverse();
                f2.second++;
                if (f2.second > 6)
                    return new IUPACNameStructureElement(7, null, null);
                lengthAndNodePairList.Add(new LengthAndListAndParentNodePair(f2.second, pair.parentNode, f2.path));
            }
        }

        return new IUPACNameStructureElement(0, lengthAndNodePairList, nodeNumbersOfCycloAlkanes);
    }
}
