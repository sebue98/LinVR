using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public ButtonBoard buttonBoard;
    public int choosenCarbonForBenzeneConnectionToSend;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
            buttonBoard.SetBenzeneCarbonToConnectToMolecule(choosenCarbonForBenzeneConnectionToSend);
    }
}
