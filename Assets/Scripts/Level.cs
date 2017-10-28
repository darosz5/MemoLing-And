using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public int stars;
    public int LevelState;
    
    [SerializeField]
    private int m_levelNum;
    public int level;
    public int numOfCards;

    public float index;
    public float count;

    public LevelGoal[] LevelGoals;
    public Text LevelText;

    private void Awake()
    {
        m_levelNum = int.Parse(gameObject.name);
  }

    public void SetActiveLevel()
    {
        if (LevelState == 0)
            return;     
        LevelsManager.Instance.activeLevelNum = m_levelNum;
        LevelsManager.Instance.numOfStages = LevelGoals.Length;
        //if (!GameControl.control.IsTesting)
        //{
        //    LevelsManager.Instance.numOfCards = numOfCards;
                              
        //}
        
        LevelsManager.Instance.difficultyLevel = level;
        LevelsManager.Instance.LevelGoals = LevelGoals;

        string originalFileName = LocalizationManager.Instance.category + 
            "/"+ LocalizationManager.Instance.firstLanguage;
        string localizedFileName = LocalizationManager.Instance.category +
            "/"+ LocalizationManager.Instance.secondLanguage;

        LocalizationManager.Instance.LoadOriginalText(originalFileName);
        LocalizationManager.Instance.LoadLocalizedText(localizedFileName);
      
    }
}
