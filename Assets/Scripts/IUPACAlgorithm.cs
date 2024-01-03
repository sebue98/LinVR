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
}