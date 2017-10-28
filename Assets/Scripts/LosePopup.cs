using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour {

    public Text levelNumberText;
   
    private int levelNumber;




    public void Initialize()
    {
       
        levelNumber = LevelsManager.Instance.activeLevelNum;

        levelNumberText.text = levelNumber.ToString();
        

     
    }
}
