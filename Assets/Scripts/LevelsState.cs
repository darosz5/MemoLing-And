using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsState : MonoBehaviour {

    private GameObject[] levelsArray;

    public Color NewLevelColor;

    private void Awake()
    {
        levelsArray = new GameObject[transform.childCount];
        
        for (int i = 0; i < transform.childCount; i++)
        {
            levelsArray[i] = transform.GetChild(i).gameObject;
        }

        
       

    }

    private void OnEnable()
    {
        AudioListener.volume = PlayerPrefs.GetInt("sound_on");

        for (int i = 0; i < levelsArray.Length; i++)
        {
            Level level = levelsArray[i].GetComponent<Level>();

            level.LevelState = GameControl.control.FindCategory(LevelsManager.Instance.categoryName).levelsState[i];

            //if (GameControl.control.IsTesting)
            //{
            //    level.LevelState = 1;
            //}

            int score = GameControl.control.FindCategory(LevelsManager.Instance.categoryName).scores[i];

            level.stars = GameControl.control.GetStars(score, i);



            GameObject state = levelsArray[i].transform.GetChild(level.LevelState).gameObject;
            
            state.gameObject.SetActive(true);

            if (level.LevelState == 2)
            {
                Transform starsParent = state.transform.GetChild(state.transform.childCount - 1);
                for (int j = 0; j < level.stars; j++)
                {
                    starsParent.GetChild(j).gameObject.SetActive(true);
                }
            }
            if(level.LevelState == 1)
            {
                level.LevelText.color = NewLevelColor;
            }

            


        }
     
    }

    
}
