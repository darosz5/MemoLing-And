using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPTest : MonoBehaviour {

    string categoryName = "travel";

    string english = "german";

    List<string> lannguages;

    void Awake()
    {
        lannguages = new List<string>()
        {
           english
        };
    }

    public void AddTravelEnglish()
    {     
        CategoryAvaliability category = IsCategoryAvaliable();
        if (category == null)
        {
            GameControl.control.avaliableCategories.Add(new CategoryAvaliability(categoryName, lannguages));
        }
        else
        {
            if (!category.avaliableLanguages.Contains(english))
            {
                category.avaliableLanguages.Add(english);
            }
            else
            {
                Debug.Log("you have this");
            }
        }
        GameControl.control.SavePlayerData();
    }

    CategoryAvaliability IsCategoryAvaliable()
    {
        for (int i = 0; i < GameControl.control.avaliableCategories.Count; i++)
        {
            if (GameControl.control.avaliableCategories[i].categoryName == categoryName)
            {
                return GameControl.control.avaliableCategories[i];
            }
        }
        return null;
    }
}
