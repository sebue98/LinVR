using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbon : MonoBehaviour
{
    //Properties
    public bool Hydrogen1 { get; set; }
    public bool Hydrogen2 { get; set; }
    public bool Hydrogen3 { get; set; }
    public bool Hydrogen4 { get; set; }

    public bool Connection1 { get; set; }
    public bool Connection2 { get; set; }
    public bool Connection3 { get; set; }
    public bool Connection4 { get; set; }

    public Carbon()
    {
        Hydrogen1 = true;
        Hydrogen2 = true;
        Hydrogen3 = true;
        Hydrogen4 = true;

        Connection1 = true;
        Connection2 = true;
        Connection3 = true;
        Connection4 = true;
    }
}
