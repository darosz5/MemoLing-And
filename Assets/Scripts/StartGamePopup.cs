using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePopup : MonoBehaviour {

    public Text levelNumberText;
    public Text highScoreText;
    public GameObject stars;

    private int levelNumber;
    private int highScore;
    private int numOfStars;



    public void Initialize()
    {
        
        levelNumber = LevelsManager.Instance.activeLevelNum;

        highScore = GameControl.control.FindCategory
            (LevelsManager.Instance.categoryName).scores[levelNumber - 1];

        numOfStars = GameControl.control.GetStars(highScore, levelNumber - 1);

        levelNumberText.text = levelNumber.ToString();
        highScoreText.text = highScore.ToString();

        for (int i = 0; i < numOfStars; i++)
        {
            stars.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    

}
