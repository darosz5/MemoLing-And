using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryAvaliability
{
    public string categoryName;
    public List<string> avaliableLanguages;

    public CategoryAvaliability(string categoryName, List<string> avaliableLanguages)
    {
        this.categoryName = categoryName;
        this.avaliableLanguages = avaliableLanguages;
    }
}

[System.Serializable]
public class CategoryState
{
    public int[] levelsState;
    public int[] scores;
    public string categoryName;
    public bool loadedSprites;
    public string url;
    public int num;
    public bool isBasic;


    public CategoryState(int[] levelsState, int[] scores,
        string categoryName, bool loadedSprites, string url, int num, bool isBasic)
    {
        this.levelsState = levelsState;
        this.scores = scores;
        this.categoryName = categoryName;
        this.loadedSprites = loadedSprites;
        this.url = url;
        this.num = num;
        this.isBasic = isBasic;
    }
}


[System.Serializable]
public  class PlayerData {

    public List<CategoryAvaliability> avaliableCategories = new List<CategoryAvaliability>();
    public List<POWERUP> inventory = new List<POWERUP>();

    public bool secondLaunch;

    public string firstLanguage;
    public string secondLanguage;
    public int languageCount;
    
   

}
[System.Serializable]
public class States {

    public List<CategoryState> categoryStates;

}
