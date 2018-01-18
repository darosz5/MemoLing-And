using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[System.Serializable]
public class LevelGoal
{
    public GAME_GOAL goal;
    public int amount;
}

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager Instance;
    
    [HideInInspector]
    public int numOfStages;

    [HideInInspector]
    public int activeLevelNum;    

    [HideInInspector]
    public int difficultyLevel;

    [HideInInspector]
    public LevelGoal[] LevelGoals;

    [HideInInspector]
    public int numOfCards;

    [HideInInspector]
    public string categoryName;

    [HideInInspector]
    public AssetBundle bundle;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

   


}