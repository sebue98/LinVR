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

    public GameObject TwoMethylPropane;
    public GameObject TwoMethylButane;
    public GameObject TwoMethylPentane;
    public GameObject ThreeMethylPentane;
    public GameObject ThreeEthylPentane;

    public GameObject TwoMethylHexane;
    public GameObject ThreeMethylHexane;
    public GameObject ThreeEthylHexane;
    public GameObject TwoMethylHeptane;

    public GameObject ThreeMethylHeptane;
    public GameObject ThreeEthylHeptane;
    public GameObject FourMethylHeptane;
    public GameObject FourPropylHeptane;

    public GameObject TwoMethylNonane;
    public GameObject ThreeMethylNonane;
    public GameObject ThreeEthylNonane;
    public GameObject FourMethylNonane;
    public GameObject FourEthylNonane;

    public GameObject FourPropylNonane;
    public GameObject FiveMethylNonane;
    public GameObject FiveEthylNonane;
    public GameObject FivePropylNonane;
    public GameObject FiveButylNonane;

    public List<GameObject> mediumSolutionGameObjects = new List<GameObject>();

    public List<List<(int, int)>> pairListForShowingMediumTaskSolution = new List<List<(int, int)>> {
    new List<(int, int)> {(8, 5), (9, 5), (10, 5), (9, 6)}, //2-MethylPropan
    new List<(int, int)> {(8, 5), (9, 5), (10, 5), (11,5), (9, 6)}, //2-MethylButan
    new List<(int, int)> {(8, 5), (9, 5), (10, 5), (11,5), (12,5), (9, 6)}, //2-MethylPentan
    new List<(int, int)> {(7,5), (8,5), (9,5), (10,5), (11,5), (9,6)}, //3-MethylPentan
    new List<(int, int)> {(7,5), (8,5), (9,5), (10,5), (11,5), (9,6), (9,7) }, //3-EthylPentan
    new List<(int, int)> {(7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (8,6) }, //2-MethylHexan
    new List<(int, int)> {(7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (9,6) }, //3-MethylHexan
    new List<(int, int)> {(7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (9,6), (9,7) }, //3-EthylHexan
    new List<(int, int)> {(6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (7,6) }, //2-MethylHeptan
    new List<(int, int)> {(6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (8,6) }, //3-MethylHeptan
    new List<(int, int)> {(6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (8,6), (8,7) }, //3-EthylHeptan
    new List<(int, int)> {(6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (9,6) }, //4-MethylHeptan
    new List<(int, int)> {(6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (9,6), (9,7), (9,8) }, //4-PropylHeptan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (6,6) }, //2-MethylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (7,6) }, //3-MethylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (7,6), (7,7)}, //3-EthylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (8,6)}, //4-MethylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (8,6), (8,7)}, //4-EthylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (8,6), (8,7), (8,8)}, //4-PropylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (9,6)}, //5-MethylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (9,6), (9,7)}, //5-EthylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (9,6), (9,7), (9,8)}, //5-PropylNonan
    new List<(int, int)> {(5,5), (6,5), (7,5), (8,5), (9,5), (10,5), (11,5), (12,5), (13,5), (9,6), (9,7), (9,8), (9,9)}, //5-ButylNonan
    };

    private void Start()
    {
        mediumSolutionGameObjects.Add(TwoMethylPropane);
        mediumSolutionGameObjects.Add(TwoMethylButane);
        mediumSolutionGameObjects.Add(TwoMethylPentane);
        mediumSolutionGameObjects.Add(ThreeMethylPentane);
        mediumSolutionGameObjects.Add(ThreeEthylPentane);
        mediumSolutionGameObjects.Add(TwoMethylHexane);
        mediumSolutionGameObjects.Add(ThreeMethylHexane);
        mediumSolutionGameObjects.Add(ThreeEthylHexane);
        mediumSolutionGameObjects.Add(TwoMethylHeptane);
        mediumSolutionGameObjects.Add(ThreeMethylHeptane);
        mediumSolutionGameObjects.Add(ThreeEthylHeptane);
        mediumSolutionGameObjects.Add(FourMethylHeptane);
        mediumSolutionGameObjects.Add(FourPropylHeptane);
        mediumSolutionGameObjects.Add(TwoMethylNonane);
        mediumSolutionGameObjects.Add(ThreeMethylNonane);
        mediumSolutionGameObjects.Add(ThreeEthylNonane);
        mediumSolutionGameObjects.Add(FourMethylNonane);
        mediumSolutionGameObjects.Add(FourEthylNonane);
        mediumSolutionGameObjects.Add(FourPropylNonane);
        mediumSolutionGameObjects.Add(FiveMethylNonane);
        mediumSolutionGameObjects.Add(FiveEthylNonane);
        mediumSolutionGameObjects.Add(FivePropylNonane);
        mediumSolutionGameObjects.Add(FiveButylNonane);

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
