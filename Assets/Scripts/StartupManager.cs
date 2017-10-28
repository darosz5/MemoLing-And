using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    public GameObject StartUpScreen;

    public Category[] categories;

    void Start()
    {
        if(LevelsManager.Instance.bundle != null)
        {
            LevelsManager.Instance.bundle.Unload(true);
            LevelsManager.Instance.bundle = null;
        }    
        
    }
    
}
