using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SortingElementForCycles
{
    public int lengthOfSubstituent;
    public int positionOfSubstituentInRing;

    public SortingElementForCycles(int first, int second)
    {
        this.lengthOfSubstituent = first;
        this.positionOfSubstituentInRing = second;
    }
}

public class IUPACAlgorithm : MonoBehaviour
{
    public int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
    private GameMaster GMInstance;

    public List<GameObject> longestChainGlobal = new List<GameObject>();
    public List<List<GameObject>> neighboringChainsElements = new List<List<GameObject>>();

    void Start()
    {
        GMInstance = GameMaster.Instance;
    }

    public String CreateCycloName(IUPACNameStructureElement namingElement)
    {
        List<LengthAndListAndParentNodePair> subtreeList = namingElement.subtreeList;
        List<int> nodesOfCycloRings = namingElement.longestChainList;
        //List<string> subTreeNames = CalculateSubNamesForCycles(subtreeList, nodesOfCycloRings, true);
        List<string> subTreeNames = CalculateSubNamesForCycles(subtreeList, nodesOfCycloRings, true);
        string returnString = "";

        foreach(string name in subTreeNames)
        {
            returnString += name + "-";
        }
        if(subTreeNames.Count != 0)
            returnString.Substring(returnString.Length - 2);
        returnString += "Cyclohexane";
        return returnString;
    }

    

    public String CreateIUPACName(int typeOfConnectionSDT, IUPACNameStructureElement namingElement)
    {
        int lengthOfChain = namingElement.lenghtOfChain;
        List<LengthAndListAndParentNodePair> subtreeList = namingElement.subtreeList;
        List<int> longestChainList = namingElement.longestChainList;
        List<string> subtreeNames = CalculateSubNames(subtreeList, longestChainList, false);

        // string[] easyTasks = {"Undecan", "Pentan"}......

        string[] startMolecules = { "Methan", "Ethan", "Propan", "Butan", "Pentan", "Hexan", "Heptan", "Octan", "Nonan", "Decan", "Undecan", "Duodecan",
        "Tridecan", "Tetradecan", "Pentadecan", "Hexadecan", "Heptadecan", "Octadecan", "Nonadecan", "Eicosan", "Heneicosan"};
        string[] prefixes = {"", "Hen", "Do", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona" }; //Will only be used for molecules with size > 21
        string[] suffixes = { "n", "en", "in" }; //single connection, double connection, triple connection
        string[] middlePart = {"", "deca", "cosa", "triaconta", "tetraconta", "pentaconta", "hexaconta", "heptaconta", "octaconta", "nonaconta"};

        string lengthName = "";
        if(subtreeNames.Count > 0)
        {
            foreach (string name in subtreeNames)
            {
                lengthName += name + "-";
            }
            lengthName.Substring(lengthName.Length - 2);
        }

        if(lengthOfChain < 22)
        {
            lengthName += startMolecules[lengthOfChain - 1];
        }
        else if(lengthOfChain >= 22 && lengthOfChain < 100)
        {
            int tensPlace = (lengthOfChain / 10) % 10;
            int onesPlace = lengthOfChain % 10;

            lengthName += prefixes[onesPlace];
            lengthName += middlePart[tensPlace];

            lengthName += suffixes[typeOfConnectionSDT];
        }
        else
        {
            int tensPlace = (lengthOfChain / 10) % 10;
            int onesPlace = lengthOfChain % 10;

            lengthName += prefixes[onesPlace];
            lengthName += middlePart[tensPlace];

            lengthName += "hecta" + suffixes[typeOfConnectionSDT];
        }

        if(GameMaster.Instance.onlyShowTaskName)
        {
            if(!lengthName.Equals(GameMaster.Instance.currentEasyTaskToSolve))
            {
                GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                return GameMaster.Instance.currentEasyTaskToSolve;
            }
            else
            {
                GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                GameMaster.Instance.easyTasksSolved++;
                GameMaster.Instance.easyTaskCounterTextMeshProComponent.text = GameMaster.Instance.easyTasksSolved.ToString() + "/3";
                GameMaster.Instance.easyTaskButton.interactable = true;
                GameMaster.Instance.onlyShowTaskName = false;
                if (GameMaster.Instance.easyTasksSolved == 3)
                {
                    GameMaster.Instance.easyTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    GameMaster.Instance.easyTaskButton.interactable = false;
                }

                //PlayerPrefs.SetInt("easyTaskScore", GameMaster.Instance.easyTasksSolved);
                GameMaster.Instance.OnResetDrawingBoard();
                return GameMaster.Instance.currentEasyTaskToSolve;
            }
        }
        else
        {
            return lengthName;
        }
    }

    static List<int> CreateList(int length)
    {
        List<int> result = new List<int>();

        for (int i = 1; i <= length; i++)
        {
            result.Add(i);
        }

        return result;
    }

    public List<string> CalculateSubNamesForCycles(List<LengthAndListAndParentNodePair> subtreeList, List<int> longestChainList, bool CycloOnly)
    {
        string[] branchedChainNames = { "", "Methyl", "Ethyl", "Propyl", "Butyl", "Pentyl", "Hexyl", "Heptyl", "Octyl", "Nonyl" };
        string[] prefixesForSubstituents = { "", "", "Di", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona", "Deca", "Undeca", "Duodeca" };
        List<int> integersForNamingList = CreateList(longestChainList.Count());
        List<string> iupacSubtreeNames = new List<string>();
        SortedDictionary<string, List<int>> nameAndPositionsOfSubstituent = new SortedDictionary<string, List<int>>();
        List<SortingElementForCycles> cycleSortingList = new List<SortingElementForCycles>()
        {
             new SortingElementForCycles(0,0), new SortingElementForCycles(0,0), new SortingElementForCycles(0,0),
             new SortingElementForCycles(0,0), new SortingElementForCycles(0,0), new SortingElementForCycles(0,0)
        };
        //Max amount = 21

        foreach(var subtree in subtreeList)
        {
            cycleSortingList[subtree.parentNode].positionOfSubstituentInRing = subtree.parentNode;
            cycleSortingList[subtree.parentNode].lengthOfSubstituent = subtree.length;
        }

        int maxNumber = 21;
        List<SortingElementForCycles> sortingListForNaming = new List<SortingElementForCycles>()
                    {
             new SortingElementForCycles(0,0), new SortingElementForCycles(0,0), new SortingElementForCycles(0,0),
             new SortingElementForCycles(0,0), new SortingElementForCycles(0,0), new SortingElementForCycles(0,0)
        };
        //int bestPositionToStart = 0;
        //Calculating length
        for (int i = 0; i < 12; i++)
        {
            //Calculating a round of numbers
            int currentPositionNumbering = 0;
            for(int sub = 1; sub < 7; sub++)
            {
                if(cycleSortingList[sub-1].lengthOfSubstituent > 0)
                {
                    currentPositionNumbering += sub;
                }
            }

            //Resetting value if numbering is smaller
            if (currentPositionNumbering < maxNumber)
            {
                maxNumber = currentPositionNumbering;
                for(int j = 0; j < 6; j++)
                {
                    sortingListForNaming[j] = cycleSortingList[j];
                }
            }

            //Moving elements one position so we can redo everything
            SortingElementForCycles temp = cycleSortingList.ElementAt(0);
            cycleSortingList.RemoveAt(0);
            cycleSortingList.Add(temp);

            if (i == 5)
                cycleSortingList.Reverse();
        }

        for(int index = 1; index < 7; index++)
        {
            if (sortingListForNaming[index-1].lengthOfSubstituent == 0)
                continue;
            if(!nameAndPositionsOfSubstituent.ContainsKey(branchedChainNames[sortingListForNaming[index-1].lengthOfSubstituent]))
            {
                nameAndPositionsOfSubstituent[branchedChainNames[sortingListForNaming[index-1].lengthOfSubstituent]] = new List<int>() {index};
                nameAndPositionsOfSubstituent[branchedChainNames[sortingListForNaming[index-1].lengthOfSubstituent]].Sort();
            }
            else
            {
                nameAndPositionsOfSubstituent[branchedChainNames[sortingListForNaming[index-1].lengthOfSubstituent]].Add(index);
                nameAndPositionsOfSubstituent[branchedChainNames[sortingListForNaming[index-1].lengthOfSubstituent]].Sort();
            }
        }

        foreach (var substituent in nameAndPositionsOfSubstituent)
        {
            if (substituent.Value.Count > 1)
            {
                string numbers = "";
                foreach (var number in substituent.Value)
                {
                    numbers += number.ToString() + ",";
                }
                iupacSubtreeNames.Add(numbers + "-" + prefixesForSubstituents[substituent.Value.Count] + substituent.Key.ToString());
            }
            else
            {
                foreach (var number in substituent.Value)
                    iupacSubtreeNames.Add(number + "-" + substituent.Key.ToString());
            }

        }


        return iupacSubtreeNames;
    }

    public List<string> CalculateSubNames(List<LengthAndListAndParentNodePair> subtreeList, List<int> longestChainList, bool CycloOnly)
    {
        string[] branchedChainNames = {"", "Methyl", "Ethyl", "Propyl", "Butyl", "Pentyl", "Hexyl", "Heptyl", "Octyl", "Nonyl" };
        string[] prefixesForSubstituents = { "", "", "Di", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona", "Deca", "Undeca", "Duodeca" };
        //Sorted alphabetically: Butyl, Ethyl, Heptyl, Hexyl, Methyl, Nonyl, Octyl, Pentyl, Propyl
        List<string> iupacSubtreeNames = new List<string>();
        List<int> integersForNamingList = CreateList(longestChainList.Count());
        SortedDictionary<string, List<int>> nameAndPositionsOfSubstituent = new SortedDictionary<string, List<int>>();

        //Get the subtree with the longest path and its index
        int maxSubLength = 0;
        for(int i = 0; i < subtreeList.Count; i++)
        {
            if(subtreeList[i].length > maxSubLength)
            {
                maxSubLength = subtreeList[i].length;
            }
        }

        int lengthOfChain = longestChainList.Count();
       
        if (lengthOfChain % 2 == 0) //The chain has an even amount of items = no concrete middle
        {
            int lowerMidPoint = lengthOfChain / 2;
            int higherMidPoint = lowerMidPoint + 1;
            int numberToDepictNamingReversing = 0; //If <=0, naming will not be reversed, if >=1, naming will be reversed
            foreach (var subtree in subtreeList)
            {
                if (integersForNamingList[longestChainList.IndexOf(subtree.parentNode)] >= higherMidPoint)
                    numberToDepictNamingReversing++;
                else
                    numberToDepictNamingReversing--;
            }
            Debug.Log(numberToDepictNamingReversing);
            if (numberToDepictNamingReversing > 0) //Reverse naming
            {
                integersForNamingList.Reverse();
            }

            foreach (var subtree in subtreeList)
            {
                int numberForNaming = integersForNamingList[longestChainList.IndexOf(subtree.parentNode)];
                if (!nameAndPositionsOfSubstituent.ContainsKey(branchedChainNames[subtree.length]))
                {
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]] = new List<int>() { numberForNaming };
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Sort();
                }
                else
                {
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Add(numberForNaming);
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Sort();
                }
            }

            if(GameMaster.Instance.benzenObjectsInTree.Count > 0 && !CycloOnly)
            {
                foreach(var benzene in GameMaster.Instance.benzenObjectsInTree)
                {
                    int numberForNaming = integersForNamingList[longestChainList.IndexOf(benzene.parentNodeInChain)];
                    if (!nameAndPositionsOfSubstituent.ContainsKey("Cyclohexyl"))
                    {
                        nameAndPositionsOfSubstituent["Cyclohexyl"] = new List<int>() { numberForNaming };
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Sort();
                    }
                    else
                    {
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Add(numberForNaming);
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Sort();
                    }
                }
            }

            foreach (var substituent in nameAndPositionsOfSubstituent)
            {
                if (substituent.Value.Count > 1)
                {
                    string numbers = "";
                    foreach (var number in substituent.Value)
                    {
                        numbers += number.ToString() + ",";
                    }
                    iupacSubtreeNames.Add(numbers + "-" + prefixesForSubstituents[substituent.Value.Count] + substituent.Key.ToString());
                }
                else
                {
                    foreach (var number in substituent.Value)
                        iupacSubtreeNames.Add(number + "-" + substituent.Key.ToString());
                }

            }
        }
        else //Else the number is uneven and we have a concrete middle point
        {
            int midPoint = (lengthOfChain / 2) + 1;
            int numberToDepictNamingReversing = 0; //If <=0, naming will not be reversed, if >=1, naming will be reversed

            foreach(var subtree in subtreeList)
            {
                if (integersForNamingList[longestChainList.IndexOf(subtree.parentNode)] > midPoint)
                    numberToDepictNamingReversing++;
                else
                    numberToDepictNamingReversing--;
            }
            if (numberToDepictNamingReversing > 0) //Reverse naming
            {
                integersForNamingList.Reverse();
            }

            foreach (var subtree in subtreeList)
            {
                int numberForNaming = integersForNamingList[longestChainList.IndexOf(subtree.parentNode)];
                if (!nameAndPositionsOfSubstituent.ContainsKey(branchedChainNames[subtree.length]))
                {
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]] = new List<int>() { numberForNaming };
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Sort();
                }
                else
                {
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Add(numberForNaming);
                    nameAndPositionsOfSubstituent[branchedChainNames[subtree.length]].Sort();
                }
            }

            if (GameMaster.Instance.benzenObjectsInTree.Count > 0 && !CycloOnly)
            {
                foreach (var benzene in GameMaster.Instance.benzenObjectsInTree)
                {
                    int numberForNaming = integersForNamingList[longestChainList.IndexOf(benzene.parentNodeInChain)];
                    if (!nameAndPositionsOfSubstituent.ContainsKey("Cyclohexyl"))
                    {
                        nameAndPositionsOfSubstituent["Cyclohexyl"] = new List<int>() { numberForNaming };
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Sort();
                    }
                    else
                    {
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Add(numberForNaming);
                        nameAndPositionsOfSubstituent["Cyclohexyl"].Sort();
                    }
                }
            }

            foreach (var substituent in nameAndPositionsOfSubstituent)
            {
                if (substituent.Value.Count > 1)
                {
                    string numbers = "";
                    foreach (var number in substituent.Value)
                    {
                        numbers += number + ",";
                    }
                    iupacSubtreeNames.Add(numbers + "-" + prefixesForSubstituents[substituent.Value.Count] + substituent.Key.ToString());
                }
                else
                {
                    foreach (var number in substituent.Value)
                        iupacSubtreeNames.Add(number + "-" + substituent.Key.ToString());
                }

            }
        }


        return iupacSubtreeNames;
    }
}