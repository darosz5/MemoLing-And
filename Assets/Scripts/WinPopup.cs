using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour {

    public Text levelNumberText;
    public Text highScoreText;
    public GameObject stars;

    private int levelNumber;
    private int numOfStars;

 



    public void Initialize(int score)
    {

        levelNumber = LevelsManager.Instance.activeLevelNum;

      
        numOfStars = GameControl.control.GetStars(score, levelNumber - 1);

        levelNumberText.text = levelNumber.ToString();
        highScoreText.text = score.ToString();

        for (int i = 0; i < numOfStars; i++)
        {
            stars.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

}
