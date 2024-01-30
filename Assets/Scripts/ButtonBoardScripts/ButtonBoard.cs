using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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
    public TextMeshProUGUI taskDecisionBoardTextMeshProComponent;
    public TextMeshProUGUI IUPACNameBoardTextMeshProComponent;
    public Button easyTaskButton;
    public Button mediumTaskButton;
    public Button hardTaskButton;
    public Button SkipTaskButton;


    public Slider benzeneConnectionSlider;
    public TextMeshProUGUI benzeneConnectionNumberHolder;
    
    private GameMaster GMInstance;
    private Color errorColorMaterial = new Color(255,82,90);

    public List<int> lastEasyTaskNumber;
    public List<int> lastMediumTaskNumber;
    public List<int> lastHardTaskNumber;

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
        if (GameMaster.Instance.easyTaskChoosen || GameMaster.Instance.mediumTaskChoosen || GameMaster.Instance.hardTaskChoosen)
            GameMaster.Instance.OnResetDrawingBoardWhenTaskActive();
        else
            GameMaster.Instance.OnResetDrawingBoard(true);
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

    //TaskDecisionBoardFunctions
    public void ToggleTaskDecisionBoard()
    {
        GameMaster.Instance.toggleTaskDecisionBoard = !GameMaster.Instance.toggleTaskDecisionBoard;
        GameMaster.Instance.taskDecisionBoard.SetActive(GameMaster.Instance.toggleTaskDecisionBoard);
        if(GameMaster.Instance.toggleTaskDecisionBoard)
        {
            taskDecisionBoardTextMeshProComponent.text = "Stop Naming Task";
        }
        else
        {
            taskDecisionBoardTextMeshProComponent.text = "Start Naming Task";
            GameMaster.Instance.OnResetDrawingBoard(true);
            IUPACNameBoardTextMeshProComponent.text = "";
            GameMaster.Instance.currentState = State.Delete;
        }
    }

    static int GetRandomIndex(int arrayLength)
    {
        System.Random random = new System.Random();
        return random.Next(0, arrayLength);
    }

    public void SetEasyTask()
    {
        if (GameMaster.Instance.mediumTaskChoosen || GameMaster.Instance.hardTaskChoosen)
            GameMaster.Instance.OnResetDrawingBoardWhenOtherTaskActive();
        else
            GameMaster.Instance.OnResetDrawingBoardWhenTaskActive();
        GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        int easyNumber = GetRandomIndex(GameMaster.Instance.easyTasks.Count);
        while(lastEasyTaskNumber.Contains(easyNumber))
            easyNumber = GetRandomIndex(GameMaster.Instance.easyTasks.Count);

        lastEasyTaskNumber.Add(easyNumber);
        GameMaster.Instance.currentEasyTaskToSolve = GameMaster.Instance.easyTasks[easyNumber];
        IUPACNameBoardTextMeshProComponent.text = GameMaster.Instance.currentEasyTaskToSolve;
        GameMaster.Instance.onlyShowTaskName = true;
        easyTaskButton.interactable = false;
        GameMaster.Instance.easyTaskChoosen = true;
    }
    public void SetMediumTask()
    {
        if (GameMaster.Instance.easyTaskChoosen || GameMaster.Instance.hardTaskChoosen)
            GameMaster.Instance.OnResetDrawingBoardWhenOtherTaskActive();
        else
            GameMaster.Instance.OnResetDrawingBoardWhenTaskActive();
        GameMaster.Instance.OnResetDrawingBoard(false);
        GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        int mediumNumber = GetRandomIndex(GameMaster.Instance.mediumTasks.Count);
        while (lastMediumTaskNumber.Contains(mediumNumber))
            mediumNumber = GetRandomIndex(GameMaster.Instance.mediumTasks.Count);

        lastMediumTaskNumber.Add(mediumNumber);
        GameMaster.Instance.currentMediumTaskToSolve = GameMaster.Instance.mediumTasks[mediumNumber];
        IUPACNameBoardTextMeshProComponent.text = GameMaster.Instance.currentMediumTaskToSolve;
        GameMaster.Instance.onlyShowTaskName = true;
        mediumTaskButton.interactable = false;
        GameMaster.Instance.mediumTaskChoosen = true;
    }
    public void SetHardTask()
    {
        if (GameMaster.Instance.mediumTaskChoosen || GameMaster.Instance.mediumTaskChoosen)
            GameMaster.Instance.OnResetDrawingBoardWhenOtherTaskActive();
        else
            GameMaster.Instance.OnResetDrawingBoardWhenTaskActive();
        GameMaster.Instance.OnResetDrawingBoard(false);
        GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        int hardNumber = GetRandomIndex(GameMaster.Instance.hardTasks.Count);
        while (lastHardTaskNumber.Contains(hardNumber))
            hardNumber = GetRandomIndex(GameMaster.Instance.hardTasks.Count);

        lastHardTaskNumber.Add(hardNumber);
        GameMaster.Instance.currentHardTaskToSolve = GameMaster.Instance.hardTasks[hardNumber];
        IUPACNameBoardTextMeshProComponent.text = GameMaster.Instance.currentHardTaskToSolve;
        GameMaster.Instance.onlyShowTaskName = true;
        hardTaskButton.interactable = false;
        GameMaster.Instance.hardTaskChoosen = true;
    }

    public void SkipTask()
    {
        string taskToShow = "";
        int taskChooser = 0;
        List<(int, int)> solutionToShow = new List<(int, int)>();
        if(GameMaster.Instance.easyTaskChoosen)
        {
            taskChooser = 1;
            taskToShow = GameMaster.Instance.currentEasyTaskToSolve;
            GameMaster.Instance.easyTasksSolved++;
            GameMaster.Instance.easyTaskCounterTextMeshProComponent.text = GameMaster.Instance.easyTasksSolved.ToString() + "/3";
            GameMaster.Instance.easyTaskButton.interactable = true;
            GameMaster.Instance.onlyShowTaskName = false;
            if (GameMaster.Instance.easyTasksSolved == 3)
            {
                GameMaster.Instance.easyTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                GameMaster.Instance.easyTaskButton.interactable = false;
            }
        }
        if (GameMaster.Instance.mediumTaskChoosen)
        {
            taskChooser = 2;
            taskToShow = GameMaster.Instance.currentMediumTaskToSolve;
            GameMaster.Instance.mediumTasksSolved++;
            GameMaster.Instance.mediumTaskCounterTextMeshProComponent.text = GameMaster.Instance.mediumTasksSolved.ToString() + "/3";
            GameMaster.Instance.mediumTaskButton.interactable = true;
            GameMaster.Instance.onlyShowTaskName = false;
            if (GameMaster.Instance.mediumTasksSolved == 3)
            {
                GameMaster.Instance.mediumTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                GameMaster.Instance.mediumTaskButton.interactable = false;
            }
        }
        if (GameMaster.Instance.hardTaskChoosen)
        {
            taskChooser = 3;
            taskToShow = GameMaster.Instance.currentHardTaskToSolve;
            GameMaster.Instance.hardTasksSolved++;
            GameMaster.Instance.hardTaskCounterTextMeshProComponent.text = GameMaster.Instance.hardTasksSolved.ToString() + "/3";
            GameMaster.Instance.hardTaskButton.interactable = true;
            GameMaster.Instance.onlyShowTaskName = false;
            if (GameMaster.Instance.hardTasksSolved == 3)
            {
                GameMaster.Instance.hardTaskCounterButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                GameMaster.Instance.hardTaskButton.interactable = false;
            }
        }
        GameMaster.Instance.easyTaskChoosen = false;
        GameMaster.Instance.mediumTaskChoosen = false;
        GameMaster.Instance.hardTaskChoosen = false;
        GameMaster.Instance.OnResetDrawingBoard(true);
        GameMaster.Instance.ShowTaskSolution(solutionToShow, taskToShow, taskChooser);
        GameMaster.Instance.IUPACNameBoardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
