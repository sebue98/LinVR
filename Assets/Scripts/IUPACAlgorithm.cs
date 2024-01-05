using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


public class IUPACAlgorithm : MonoBehaviour
{
    public List<GameObject> longestChainElements = new List<GameObject>();

    public List<int> longestChainNodes = new List<int>();

    public int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

    public List<GameObject> longestChainGlobal = new List<GameObject>();
    public List<List<GameObject>> neighboringChainsElements = new List<List<GameObject>>();

    public String CreateIUPACName(int typeOfConnectionSDT, IUPACNameStructureElement namingElement)
    {
        int lengthOfChain = namingElement.lenghtOfChain;
        List<LengthAndListAndParentNodePair> subtreeList = namingElement.subtreeList;
        List<int> longestChainList = namingElement.longestChainList;
        List<string> subtreeNames = CalculateSubNames(subtreeList, longestChainList);


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
            return lengthName += startMolecules[lengthOfChain - 1];
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

        return lengthName;
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

    public List<string> CalculateSubNames(List<LengthAndListAndParentNodePair> subtreeList, List<int> longestChainList)
    {
        string[] branchedChainNames = {"", "Methyl", "Ethyl", "Propyl", "Butyl", "Pentyl", "Hexyl", "Heptyl", "Octyl", "Nonyl" };
        string[] prefixesForSubstituents = { "", "", "Di", "Tri", "Tetra", "Penta", "Hexa", "Hepta", "Octa", "Nona", "Deca", "Undeca", "Duodeca" };
        //Sorted alphabetically: Butyl, Ethyl, Heptyl, Hexyl, Methyl, Nonyl, Octyl, Pentyl, Propyl
        List<string> iupacSubtreeNames = new List<string>();
        List<int> integersForNamingList = CreateList(longestChainList.Count());
        SortedDictionary<string, List<int>> nameAndPositionsOfSubstituent = new SortedDictionary<string, List<int>>();

        //Get the subtree with the longest path and its index
        int maxSubLength = 0;
        int indexOfLongestSubtree = 0;
        for(int i = 0; i < subtreeList.Count; i++)
        {
            if(subtreeList[i].length > maxSubLength)
            {
                maxSubLength = subtreeList[i].length;
                indexOfLongestSubtree = i;
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
            //int midPointNode = longestChainList[midPoint - 1];
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