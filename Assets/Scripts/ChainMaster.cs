using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMaster : MonoBehaviour
{
    private static ChainMaster _instance;

    public static ChainMaster Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy this instance if another one already exists
            Destroy(gameObject);
        }
    }

    public List<Carbon> carbons;
}
