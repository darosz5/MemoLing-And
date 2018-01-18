using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ricimi;
using UnityEngine.UI;

public class Category : MonoBehaviour {

    SceneTransition m_transition;

    [Header("Variables")]

    public string CategoryName;   

    public string Url;

    public bool IsBasic;

    public int orderNumber;

    void Awake()
    {
        m_transition = GetComponent<SceneTransition>();      
    }

    IEnumerator GetAssetBundle()
    {
        if(LevelsManager.Instance.bundle != null)
            Unload(LevelsManager.Instance.bundle);

        CategoryState category = GameControl.control.FindCategory(CategoryName);
        string filePath;
        if (category.isBasic)
        {         
            if (Application.platform == RuntimePlatform.Android)
            {
                filePath = "jar:file://" + Application.dataPath + "!/assets/" + category.categoryName;               
            }
            else
            {                   
                filePath = Application.dataPath + "/StreamingAssets/" + category.categoryName;
            }
            LevelsManager.Instance.bundle = AssetBundle.LoadFromFile(filePath);
            m_transition.PerformTransition();
        }

        else
        {
            filePath = category.url;
            WWW www = WWW.LoadFromCacheOrDownload(filePath, category.num);

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
            }
            else
            {
                LevelsManager.Instance.bundle = www.assetBundle;
                m_transition.PerformTransition();
            }
        }      
    }

    public void Inactivate()
    {
        Image image = GetComponentsInChildren<Image>()[0];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;
        GetComponent<AnimatedButton>().interactable = false;
    }

    public void SetCategory()
    {       
        if(LocalizationManager.Instance.category != null)
        {
            LocalizationManager.Instance.category = CategoryName; 
        }

        LevelsManager.Instance.categoryName = CategoryName;

        if(GameControl.control.FindCategory(CategoryName) == null)
        {
            GameControl.control.AddCategory(CategoryName, Url, IsBasic);
                    
        }
        StartCoroutine(GetAssetBundle());            
    }

    void Unload(AssetBundle bundle)
    {
        bundle.Unload(true);
        bundle = null;
    }  
}
