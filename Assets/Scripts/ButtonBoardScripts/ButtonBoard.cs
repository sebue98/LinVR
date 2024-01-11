using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Button closeInfoTextButton;
    public TextMeshProUGUI selectedModeTextMeshProComponent;
    public Button errorMessageButton;
    public TextMeshProUGUI errorMessageTextMeshProComponent;
    public TextMeshProUGUI orientationButtonTextMeshProComponent;

    public Slider benzeneConnectionSlider;
    public TextMeshProUGUI benzeneConnectionNumberHolder;
    
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
        //Activating Benzene Decision Board
        if(GMInstance.currentState == State.benzene && GMInstance.setBenzeneBoardActive)
        {
            if (GMInstance.currentorientationForMoleculeRing)
            {
                GMInstance.benzeneConnectionButtonBoardVertical.SetActive(false);
                GMInstance.benzeneConnectionButtonBoardHorizontal.SetActive(true);
            }
            else
            {
                GMInstance.benzeneConnectionButtonBoardHorizontal.SetActive(false);
                GMInstance.benzeneConnectionButtonBoardVertical.SetActive(true);
            }

        }

        //Setting Text of different display buttons
        selectedModeTextMeshProComponent.text = changeCurrentMode();
        errorMessageTextMeshProComponent.text = changeErrorMessage();
    }

    public string changeCurrentMode()
    {
        switch(GMInstance.currentState.ToString())
        {
            case "carbon":
                return "Current Mode: Carbon";
            case "benzene":
                return "Current Mode: Benzene";
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

    public void SetOrientation()
    {
        //True = Horizontal, False = Vertical
        GMInstance.currentorientationForMoleculeRing = !GMInstance.currentorientationForMoleculeRing;
        if (GMInstance.currentorientationForMoleculeRing)
            orientationButtonTextMeshProComponent.text = "Horizontal";
        else
            orientationButtonTextMeshProComponent.text = "Vertical";
    }

    public void SetBenzeneCarbonToConnectToMolecule(int choosenCarbon)
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = choosenCarbon;
    }

    public void SetChoosenBenzeneForConnectionToOne()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 1;
    }

    public void SetChoosenBenzeneForConnectionToTwo()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 2;
    }

    public void SetChoosenBenzeneForConnectionToThree()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 3;
    }

    public void SetChoosenBenzeneForConnectionToFour()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 4;
    }

    public void SetChoosenBenzeneForConnectionToFive()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 5;
    }

    public void SetChoosenBenzeneForConnectionToSix()
    {
        GMInstance.currentChoosenBenzeneCarbonForConnection = 6;
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

    public void SetDeleteMolecule()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        GameMaster.Instance.currentState = State.Delete;
    }

    public void SetBenzene()
    {
        GameMaster.Instance.currentState = State.benzene;
        GMInstance.setBenzeneBoardActive = true;
    }

    public void CloseInfoText()
    {
        GameMaster.Instance.Instuctionboard.SetActive(false);
    }

}
