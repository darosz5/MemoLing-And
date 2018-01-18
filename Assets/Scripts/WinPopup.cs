using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour {

    [Header("References")]

    public Text levelNumberText;

    public Text highScoreText;

    public GameObject stars;

    int m_levelNumber;

    int m_numberOfStars;

    public void Initialize(int score)
    {
        m_levelNumber = LevelsManager.Instance.activeLevelNum;     
        m_numberOfStars = GameControl.control.GetStars(score, m_levelNumber - 1);
        levelNumberText.text = m_levelNumber.ToString();

        highScoreText.text = score.ToString();

        for (int i = 0; i < m_numberOfStars; i++)
        {
            stars.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

}
