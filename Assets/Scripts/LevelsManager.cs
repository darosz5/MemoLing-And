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
    public int numOfStages;
    public int activeLevelNum;    
    public int difficultyLevel;
    public LevelGoal[] LevelGoals;

    public int numOfCards;

    public int[] categoryState;
    public string categoryName;
    public AssetBundle bundle;

    
    

    private static LevelsManager instance = null;

    public static LevelsManager Instance
    {
        get { return instance; }
        set { }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

   


}