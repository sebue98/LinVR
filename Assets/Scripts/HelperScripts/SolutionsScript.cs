using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionsScript : MonoBehaviour
{
    //EasySolutions
    public GameObject Methane;
    public GameObject Ethane;
    public GameObject Propane;
    public GameObject Butane;
    public GameObject Pentane;
    public GameObject Hexane;
    public GameObject Heptane;
    public GameObject Octane;
    public GameObject Nonane;
    public GameObject Decane;
    public GameObject Undecane;
    public GameObject Duodecane;

    public List<GameObject> easySolutionGameObjects = new List<GameObject>();

    public List<List<(int, int)>> pairListForShowingEasyTaskSolution = new List<List<(int, int)>> {
    new List<(int, int)> {(9, 5)}, //Methan
    new List<(int, int)> {(8,5) , (9,5) }, //Ethan
    new List<(int, int)> {(8,5) , (9,5), (10,5) }, //Propan
    new List<(int, int)> {(7,5), (8,5), (9,5),(10,5) }, //Butan
    new List<(int, int)> {(7,5), (8,5), (9,5),(10,5), (11,5)}, //Pentan
    new List<(int, int)> {(7,5), (8,5), (9,5),(10,5), (11,5), (12,5)}, //Hexan
    new List<(int, int)> {(6,5), (7,5), (8,5),(9,5),(10,5), (11,5), (12,5)}, //Heptan
    new List<(int, int)> {(6,5), (7,5), (8,5),(9,5),(10,5), (11,5), (12,5), (13,5)}, //Octan
    new List<(int, int)> {(6,5), (7,5), (8,5),(9,5),(10,5), (11,5), (12,5), (13,5), (14,5)}, //Nonan
    new List<(int, int)> {(5,5), (6,5), (7,5),(8,5),(9,5),(10,5), (11,5), (12,5), (13,5), (14,5)}, //Decan
    new List<(int, int)> {(5,5), (6,5), (7,5),(8,5),(9,5),(10,5), (11,5), (12,5), (13,5), (14,5), (15,5)}, //Undecan
    new List<(int, int)> {(5,5), (6,5), (7,5),(8,5),(9,5),(10,5), (11,5), (12,5), (13,5), (14,5), (15,5), (16,5)}}; //Duodecan

    private void Start()
    {
        easySolutionGameObjects.Add(Methane);
        easySolutionGameObjects.Add(Ethane);
        easySolutionGameObjects.Add(Propane);
        easySolutionGameObjects.Add(Butane);
        easySolutionGameObjects.Add(Pentane);
        easySolutionGameObjects.Add(Hexane);
        easySolutionGameObjects.Add(Heptane);
        easySolutionGameObjects.Add(Octane);
        easySolutionGameObjects.Add(Nonane);
        easySolutionGameObjects.Add(Decane);
        easySolutionGameObjects.Add(Undecane);
        easySolutionGameObjects.Add(Duodecane);
    }

}
