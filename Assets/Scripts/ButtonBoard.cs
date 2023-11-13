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
    public TextMeshProUGUI selectedModeTextMeshProComponent;
    public Button errorMessageButton;
    public TextMeshProUGUI errorMessageTextMeshProComponent;

    private GameMaster GMInstance;
    private Color errorColorMaterial = new Color(255,82,90);

    // Start is called before the first frame update
    void Start()
    {
        GMInstance = GameMaster.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        selectedModeTextMeshProComponent.text = changeCurrentMode();
        errorMessageTextMeshProComponent.text = changeErrorMessage();
    }

    public string changeCurrentMode()
    {
        switch(GMInstance.currentState.ToString())
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

    public string changeErrorMessage()
    {
        switch(GMInstance.currentErrorState.ToString())
        {
            case "illegalDoubleConnection":
                {
                    errorMessageButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                    return "Cannot create double connection here";
                }
            case "illegalSpawnPlace":
                {
                    errorMessageButton.GetComponent<Image>().color = new Color32(255, 82, 90, 255);
                    return "Cannot place Molecule here";
                }
            default:
                {
                    errorMessageButton.GetComponent<Image>().color = new Color(255, 255, 255);
                    return "";
                }
        }
    }

    public void TempFunction()
    {
        Debug.Log("Spawning molecule mode");
    }

    public void SetDoubleConnection()
    {
        GameMaster.Instance.currentState = State.DoubleConnection;
        GameMaster.Instance.numberDecisionBoard.SetActive(true);
    }

    public void SetTripleConnection()
    {
        GameMaster.Instance.currentState = State.TripleConnection;
        GameMaster.Instance.numberDecisionBoard.SetActive(true);
    }

    public void SetNeighbourToConnectToOne()
    {
        GameMaster.Instance.neighbourToConnectTo = 1;
    }

    public void SetNeighbourToConnectToTwo()
    {
        GameMaster.Instance.neighbourToConnectTo = 2;
    }

    public void SetNeighbourToConnectToThree()
    {
        GameMaster.Instance.neighbourToConnectTo = 3;
    }

    public void SetNeighbourToConnectToFour()
    {
        GameMaster.Instance.neighbourToConnectTo = 4;
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

    public void SetBenzene()
    {
        GameMaster.Instance.currentState = State.benzene;
    }


}
