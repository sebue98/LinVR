using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCanvasSelectObjects : MonoBehaviour
{
    public GameObject WristUI;
    bool wristUIActive = true;
    public GameObject rightHand;

    public GameObject cubeToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        DistplayWristUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DistplayWristUI()
    {
        if(wristUIActive)
        {
            WristUI.SetActive(true);
            wristUIActive = true;
        } else if (!wristUIActive)
        {
            WristUI.SetActive(false);
            wristUIActive = false;
        }
    }

    public void GenerateCube()
    {
        GameObject cube = Instantiate(cubeToInstantiate, rightHand.transform.position, Quaternion.identity);
    }
}
