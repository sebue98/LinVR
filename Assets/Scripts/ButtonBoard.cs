using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBoard : MonoBehaviour
{

    public Button deleteMoleculeButton;
    public Button setCoalButton;
    public Button setOxygenButton;
    public Button setNitrogenButton;
    public Button setSulfurButton;
    public Button setDoubleConnectionButton;
    public Button setTripleConnectionButton;
    public Button setTopButton;
    public Button setRightButton;
    public Button setBottomButton;
    public Button setLeftButton;
    public Button showCurrentModeButton;
    public TextMeshProUGUI textMeshProComponent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        textMeshProComponent.text = changeCurrentMode();
    }

    public string changeCurrentMode()
    {
        switch(GameMaster.Instance.currentState.ToString())
        {
            case "carbon":
                return "Current Mode: Carbon";
            case "oxygen":
                return "Current Mode: Oxygen";
            case "nitrogen":
                return "Current Mode: Nitrogen";
            case "sulfur":
                return "Current Mode: Sulfur";
            case "Delete":
                return "Current Mode: Delete Molecule";
            case "DoubleConnection":
                return "Current Mode: Create Double Connection";
            case "TripleConnection":
                return "Current Mode: Create Triple Connection";
            default:
                return "No mode selected";
        }
    }

    public void TempFunction()
    {
        Debug.Log("Spawning molecule mode");
    }

    public void setDoubleConnection()
    {
        GameMaster.Instance.currentState = State.DoubleConnection;
        GameMaster.Instance.numberDecisionBoard.SetActive(true);
    }

    public void setTripleConnection()
    {
        GameMaster.Instance.currentState = State.TripleConnection;
        GameMaster.Instance.numberDecisionBoard.SetActive(true);
    }

    public void SetNeighbourToConnectTo()
    {
        GameMaster.Instance.neighbourToConnectTo = 2;
    }

    public void SetCarbon()
    {
        GameMaster.Instance.currentState = State.carbon;
    }

    public void SetOxygen()
    {
        GameMaster.Instance.currentState = State.oxygen;
    }

    public void SetNitrogen()
    {
        GameMaster.Instance.currentState = State.nitrogen;
    }

    public void SetSulfur()
    {
        GameMaster.Instance.currentState = State.sulfur;
    }

    public void SetDeleteMolecule()
    {
        GameMaster.Instance.currentState = State.Delete;
    }
}
