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
    public bool breakForBenzeneCannotBeNamed = false;

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
        //GameMaster.Instance.namingStringsForTesting.Add(returnString);

        if(GameMaster.Instance.onlyShowTaskName)
        {
            if (GameMaster.Instance.hardTaskChoosen)
            {
                if (!returnString.Equals(GameMaster.Instance.currentHardTaskToSolve))
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                    return GameMaster.Instance.currentHardTaskToSolve;
                }
                else
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    GameMaster.Instance.hardTasksSolved++;
                    GameMaster.Instance.hardTaskCounterTextMeshProComponent.text = GameMaster.Instance.hardTasksSolved.ToString() + "/3";
                    GameMaster.Instance.hardTaskButton.interactable = true;
                    GameMaster.Instance.onlyShowTaskName = false;
                    if (GameMaster.Instance.hardTasksSolved == 3)
                    {
                        GameMaster.Instance.hardTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                        GameMaster.Instance.hardTaskButton.interactable = false;
                    }
                    GameMaster.Instance.OnResetDrawingBoard(false);
                    GameMaster.Instance.refreshAfterSuccesfullTask = true;
                    return GameMaster.Instance.currentHardTaskToSolve;
                }
            }
        }
       
        return returnString;
    }

    

    public String CreateIUPACName(int typeOfConnectionSDT, IUPACNameStructureElement namingElement)
    {
        int lengthOfChain = namingElement.lenghtOfChain;
        List<LengthAndListAndParentNodePair> subtreeList = namingElement.subtreeList;
        List<int> longestChainList = namingElement.longestChainList;
        List<string> subtreeNames = CalculateSubNames(subtreeList, longestChainList, false);

        string[] startMolecules = { "Methan", "Ethan", "Propan", "Butan", "Pentan", "Hexan", "Heptan", "Octan", "Nonan", "Decan", "Undecan", "Duodecan",
        "Tridecan", "Tetradecan", "Pentadecan", "Hexadecan", "Heptadecan", "Octadecan", "Nonadecan", "Eicosan", "Heneicosan"};
        string[] prefixes = {"", "Hen", "Do", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona" }; //Will only be used for molecules with size > 21
        string[] suffixes = { "n", "en", "in" }; //single connection, double connection, triple connection
        string[] middlePart = {"", "deca", "cosa", "triaconta", "tetraconta", "pentaconta", "hexaconta", "heptaconta", "octaconta", "nonaconta"};

        string lengthName = "";
        if(subtreeNames.Count > 0)
        {
            for(int i = 0; i < subtreeNames.Count-1; i++)
            {
                lengthName += subtreeNames[i] + "-";
            }
            lengthName += subtreeNames[subtreeNames.Count - 1];
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
            if(GameMaster.Instance.easyTaskChoosen)
            {
                if (!lengthName.Equals(GameMaster.Instance.currentEasyTaskToSolve))
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
                    GameMaster.Instance.OnResetDrawingBoard(false);
                    GameMaster.Instance.refreshAfterSuccesfullTask = true;
                    return GameMaster.Instance.currentEasyTaskToSolve;
                }
            }

            if(GameMaster.Instance.mediumTaskChoosen)
            {
                if (!lengthName.Equals(GameMaster.Instance.currentMediumTaskToSolve))
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                    return GameMaster.Instance.currentMediumTaskToSolve;
                }
                else
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    GameMaster.Instance.mediumTasksSolved++;
                    GameMaster.Instance.mediumTaskCounterTextMeshProComponent.text = GameMaster.Instance.mediumTasksSolved.ToString() + "/3";
                    GameMaster.Instance.mediumTaskButton.interactable = true;
                    GameMaster.Instance.onlyShowTaskName = false;
                    if (GameMaster.Instance.mediumTasksSolved == 3)
                    {
                        GameMaster.Instance.mediumTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                        GameMaster.Instance.mediumTaskButton.interactable = false;
                    }
                    GameMaster.Instance.OnResetDrawingBoard(false);
                    GameMaster.Instance.refreshAfterSuccesfullTask = true;
                    return GameMaster.Instance.currentMediumTaskToSolve;
                }
            }

            if(GameMaster.Instance.hardTaskChoosen)
            {
                if (!lengthName.Equals(GameMaster.Instance.currentHardTaskToSolve))
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                    return GameMaster.Instance.currentHardTaskToSolve;
                }
                else
                {
                    GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                    GameMaster.Instance.hardTasksSolved++;
                    GameMaster.Instance.hardTaskCounterTextMeshProComponent.text = GameMaster.Instance.hardTasksSolved.ToString() + "/3";
                    GameMaster.Instance.hardTaskButton.interactable = true;
                    GameMaster.Instance.onlyShowTaskName = false;
                    if (GameMaster.Instance.hardTasksSolved == 3)
                    {
                        GameMaster.Instance.hardTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                        GameMaster.Instance.hardTaskButton.interactable = false;
                    }
                    GameMaster.Instance.OnResetDrawingBoard(false);
                    GameMaster.Instance.refreshAfterSuccesfullTask = true;
                    return GameMaster.Instance.currentHardTaskToSolve;
                }
            }
            return "Funktion falsch abgebogen";
        }
        else
        {
            GameMaster.Instance.namingStringsForTesting.Add(lengthName);
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

        Dictionary<int, int> refactoringDicitonaryForLongestList = new Dictionary<int, int>();
        if(longestChainList[0] > 0)
        {
            int count = 0;
            foreach(var node in longestChainList)
            {
                refactoringDicitonaryForLongestList.Add(node, count);
                count++;
            }

            foreach (var subtree in subtreeList)
            {
                cycleSortingList[refactoringDicitonaryForLongestList[subtree.parentNode]].positionOfSubstituentInRing = subtree.parentNode;
                cycleSortingList[refactoringDicitonaryForLongestList[subtree.parentNode]].lengthOfSubstituent = subtree.length;
            }
        }
        else
        {
            foreach (var subtree in subtreeList)
            {
                cycleSortingList[subtree.parentNode].positionOfSubstituentInRing = subtree.parentNode;
                cycleSortingList[subtree.parentNode].lengthOfSubstituent = subtree.length;
            }
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
            if(GameMaster.Instance.benzenObjectsInTree.Count > 0 && !CycloOnly)
            {
                foreach(var cyclohexane in GameMaster.Instance.benzenObjectsInTree)
                {
                    if (longestChainList.Contains(cyclohexane.parentNodeInChain))
                    {
                        if (integersForNamingList[longestChainList.IndexOf(cyclohexane.parentNodeInChain)] >= higherMidPoint)
                            numberToDepictNamingReversing++;
                        else
                            numberToDepictNamingReversing--;
                    }
                }
            }
            foreach (var subtree in subtreeList)
            {
                if (integersForNamingList[longestChainList.IndexOf(subtree.parentNode)] >= higherMidPoint)
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

            if(GameMaster.Instance.benzenObjectsInTree.Count > 0 && !CycloOnly)
            {
                foreach(var benzene in GameMaster.Instance.benzenObjectsInTree)
                {
                    foreach (var subtree in subtreeList)
                    {
                        foreach (var node in subtree.nodeList)
                        {
                            if (node == benzene.parentNodeInChain)
                            {
                                breakForBenzeneCannotBeNamed = true;
                            }
                        }
                    }
                    if (breakForBenzeneCannotBeNamed)
                        break;
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
            breakForBenzeneCannotBeNamed = false;
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

            if (GameMaster.Instance.benzenObjectsInTree.Count > 0 && !CycloOnly)
            {
                foreach (var cyclohexane in GameMaster.Instance.benzenObjectsInTree)
                {
                    if(longestChainList.Contains(cyclohexane.parentNodeInChain))
                    {
                        if (integersForNamingList[longestChainList.IndexOf(cyclohexane.parentNodeInChain)] >= midPoint)
                            numberToDepictNamingReversing++;
                        else
                            numberToDepictNamingReversing--;
                    }
                }
            }
            foreach (var subtree in subtreeList)
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
                    foreach(var subtree in subtreeList)
                    {
                        foreach(var node in subtree.nodeList)
                        {
                            if(node == benzene.parentNodeInChain)
                            {
                                breakForBenzeneCannotBeNamed = true;
                            }
                        }
                    }
                    if (breakForBenzeneCannotBeNamed)
                        break;
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
            breakForBenzeneCannotBeNamed = false;
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