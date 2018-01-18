using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePopup : MonoBehaviour {

    [Header("References")]

    public Text levelNumberText;

    public Text highScoreText;

    public GameObject stars;

    int m_levelNumber;

    int m_highScore;

    int m_numberOfStars;

    public void Initialize()
    {       
        m_levelNumber = LevelsManager.Instance.activeLevelNum;

        m_highScore = GameControl.control.FindCategory
            (LevelsManager.Instance.categoryName).scores[m_levelNumber - 1];

        m_numberOfStars = GameControl.control.GetStars(m_highScore, m_levelNumber - 1);

        levelNumberText.text = m_levelNumber.ToString();
        highScoreText.text = m_highScore.ToString();

        for (int i = 0; i < m_numberOfStars; i++)
        {
            stars.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }    
}
