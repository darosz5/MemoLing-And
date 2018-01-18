using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour {

    public Text levelNumberText;
   
    int m_levelNumber;

    public void Initialize()
    {      
        m_levelNumber = LevelsManager.Instance.activeLevelNum;
        levelNumberText.text = m_levelNumber.ToString();           
    }
}
