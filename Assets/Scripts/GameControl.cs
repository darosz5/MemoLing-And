using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Ricimi;
using ExaGames.Common;
using UnityEngine.Networking;


[System.Serializable]
public class StarsValues
{
    public int Star1;
    public int Star2;
    public int Star3;
}

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    [Header("References")]
    [Space(10)]

    [Header("Boosters")]

    public List<Booster> activeBoosters;

    [HideInInspector]
    public GameObject SelectedWheelItem;

    [HideInInspector]
    public WheelIten deathWheelItem;

    [Header("Popups")]

    public GameObject LostConnectionPopup;

    public GameObject DemoScenePopup;

    public GameObject LanguageSelectPopup;

    public GameObject TestingPanel;

    public GameObject LoadingScreen;

    GameObject clonedLoadingScreen;

    PopupOpener m_popupOpener;

    AudioSource m_audio;

    [Header("Variables")]
    [Space(10)]

    public List<string> languages;

    [Header("Player Info")]

    [HideInInspector]
    public List<POWERUP> inventory;

    [HideInInspector]
    public List<CategoryAvaliability> avaliableCategories;
        
    [HideInInspector]
    public bool secondLaunch;

    [HideInInspector]
    public List<CategoryState> CategoryStates;

    [HideInInspector]
    public string firstLanguage;

    [HideInInspector]
    public string secondLanguage;
   
    int m_languegeCount;

    bool m_wasLastHomeScene;

    [Header("Other")]

    public StarsValues[] levelStarsValues;

    public float TimeOut;

    public int numOfCards;

    public float checkingInterval;

    public Vector3 FirstCategoryPosition;

    [HideInInspector]
    public bool IsPlayingDemo;

    [HideInInspector]
    public bool IsChecking;

    [HideInInspector]
    public bool WasLevelWon;

    [HideInInspector]
    public bool isLanguageAvalibleForSpeech;

    int m_baseCategoriesCount;

    [Header("Testing")]

    public float timeModfier;

    public float matchNumModifier;

    public float TestingPlayTime;

    [HideInInspector]
    public bool isLoggedIn = false;

    WaitForSeconds WFSChecking;

    bool m_isTimeOut;

    void Awake()
    {
        MakePersistentSingleton();
        GetReferences();
        Initialize();
    }

    void MakePersistentSingleton()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        WFSChecking = new WaitForSeconds(checkingInterval);
        m_wasLastHomeScene = true;
        if (!isLoggedIn)
        {
            GPSManager.Activate();
        }
    }

    void GetReferences()
    {
        m_audio = GetComponent<AudioSource>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    void OnEnable()
    {
        Load(PlayerPrefs.GetString("language"));

        if (!secondLaunch)
        {
            Invoke("OpenFirstLanguagePopup", .1f);
        }
    }

    void OpenFirstLanguagePopup()
    {
        m_popupOpener.popupPrefab = LanguageSelectPopup;
        m_popupOpener.OpenPopup();
    }

    public void Checking()
    {
        IsChecking = true;
    }

    public void ShowDemoPopup()
    {
        Popup[] popups = GameObject.FindObjectOfType<Canvas>()
            .GetComponentsInChildren<Popup>();
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].Close();
        }

        m_popupOpener.popupPrefab = DemoScenePopup;
        m_popupOpener.OpenPopup();
    }

    void OnLevelWasLoaded(int level)
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Categories"))
        {
            //Create the category avaliability list
            Category[] categories = Resources.LoadAll<Category>("Categories");
           

            for (int i = 0; i < categories.Length; i++)
            {
                if (categories[i].IsBasic)
                {
                    CategoryAvaliability categoryAvaliability = CheckIfHasThisCategoryAvailable(categories[i].CategoryName);
                    if (categoryAvaliability == null)
                    {
                        avaliableCategories.Add(new CategoryAvaliability(categories[i].CategoryName, languages));
                    }
                    else
                    {
                        for (int j = 0; j < languages.Count; j++)
                        {
                            if (!categoryAvaliability.avaliableLanguages.Contains(languages[j]))
                            {
                                categoryAvaliability.avaliableLanguages.Add(languages[j]);
                            }
                        }
                    }
                }                          
            }

            //Create the categories list based on avaliability
            List<Category> avaliableCategoriesList = new List<Category>();
            for (int i = 0; i < categories.Length; i++)
            {              
                for (int j = 0; j < avaliableCategories.Count; j++)
                {
                    if(avaliableCategories[j].categoryName == categories[i].name)
                    {
                        if (avaliableCategories[j].avaliableLanguages.Contains(LocalizationManager.Instance.secondLanguage))
                        {
                            avaliableCategoriesList.Add(categories[i]);
                            break;
                        }
                    }
                }
               
            }

            AddNewCategories(avaliableCategoriesList);
            PlaceCategoriesRectTransforms(avaliableCategoriesList);
            
            if (m_wasLastHomeScene)
            {
                
                InitializeBundlesLoad(CategoryStates);
                StartCoroutine(CheckIfAllLoaded());
            }
            
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HomeScene 1"))
        {
            m_wasLastHomeScene = true;
        }
        else
        {
            m_wasLastHomeScene = false;
        }
    }

    CategoryAvaliability CheckIfHasThisCategoryAvailable(string name)
    {
        for (int i = 0; i < avaliableCategories.Count; i++)
        {
            if (avaliableCategories[i].categoryName == name)
                return avaliableCategories[i];
        }
        return null;
    }

    void PlaceCategoriesRectTransforms(List<Category> categories)
    {
        List<Category> sortedCategories = categories.OrderBy(x => x.orderNumber).ToList();
       
        Transform parent = GameObject.Find("Canvas").transform.GetChild(2);
               
        SetCategoriesMoverNumOfPages(sortedCategories.Count);    

        Vector3 starting6pos = FirstCategoryPosition;

        for (int i = 0; i < sortedCategories.Count; i+=6)
        {
            if (i != 0)
            {
                starting6pos += Vector3.right * 1500f;
            }
            
            for (int j = 0; j < 6; j++)
            {             
                Vector3 startingpos = starting6pos + Vector3.down * 368f * j;

                if (j >= 3)
                {
                    startingpos += new Vector3(518f, 3 * 368f, 0f);
                }
                if (j + i == sortedCategories.Count)
                    return;

                RectTransform rectTransform = sortedCategories[i + j].GetComponent<RectTransform>();               
                rectTransform.anchoredPosition = startingpos;
                Instantiate(rectTransform.gameObject, parent, false);                               
            }
        }       
    }

    void AddNewCategories(List<Category> categories)
    {    
        for (int i = 0; i < categories.Count; i++)
        {
            if (FindCategory(categories[i].CategoryName) == null)
            {
                AddCategory(categories[i].CategoryName, categories[i].Url, categories[i].IsBasic);
            }
        }
    }

    void SetCategoriesMoverNumOfPages(int num)
    {        
        int numOfPages = (int)(num / 6) + 1;
        if (num % 6 == 0)
        {
            numOfPages--;
        }
        CategoriesMover mover = FindObjectOfType<CategoriesMover>();
        mover.numOfPages = numOfPages;
        mover.Initialize();
    }

    public int GetStars(int score, int levelNum)
    {
        int stars = 0;
        if (score < GameControl.control.levelStarsValues[levelNum].Star1)
            stars = 0;
        else if (score < GameControl.control.levelStarsValues[levelNum].Star2)
            stars = 1;
        else if (score < GameControl.control.levelStarsValues[levelNum].Star3)
            stars = 2;
        else if (score > GameControl.control.levelStarsValues[levelNum].Star3)
            stars = 3;
        return stars;
    }

    public void SaveCategory(string name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath
            + "/" + name + ".dat", FileMode.Create);
        States states = new States();       
        states.categoryStates = CategoryStates;      
        bf.Serialize(file, states);
        file.Close();
    }

    public void SavePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath
            + "/playerData.dat" , FileMode.Create);
        PlayerData playerData = new PlayerData();
        playerData.avaliableCategories = avaliableCategories;
        playerData.inventory = inventory;       
        playerData.secondLaunch = secondLaunch;
        playerData.firstLanguage = firstLanguage;
        playerData.secondLanguage = secondLanguage;
        playerData.languageCount = m_languegeCount;
        bf.Serialize(file, playerData);
        file.Close();
    }
  
    void InitializeBundlesLoad(List<CategoryState> categoryStates)
    {        
        for (int i = 0; i < categoryStates.Count; i++)
        {
            if (!categoryStates[i].loadedSprites)
            {
                StartCoroutine(LoadFirstAssetBundle(categoryStates[i]));               
            }         
        }         
    }

    public void SwitchLoadingScreen(bool on)
    {
        if (on)
        {
            if(clonedLoadingScreen == null)
            {
                clonedLoadingScreen = (GameObject)Instantiate(LoadingScreen);
                clonedLoadingScreen.transform.SetParent(GameObject.Find("Canvas")
                    .transform, false);
            }
            else
            {
                clonedLoadingScreen.SetActive(true);
            }
        }
        else
        {
            if (clonedLoadingScreen != null)
            {

                clonedLoadingScreen.GetComponent<LoadingScreen>().Hide();
            }
        }
    }

    public IEnumerator CheckIfAllLoaded()
    {       
        float timer = 0;
        int count = 0;
        SwitchLoadingScreen(true);
        SpeachManager.Instance.SetSpeach();

        if (isLanguageAvalibleForSpeech && Application.platform == RuntimePlatform.Android &&
            LocalizationManager.Instance.WasLanguageChanged)
        {         
            TTSManager.Speak("Start", false, TTSManager.STREAM.Music, 0f);
            while (!TTSManager.IsSpeaking())
            {
                yield return WFSChecking;
                timer += checkingInterval;
 
                if (timer >= TimeOut)
                {
                    break;
                }
            }
        }
       
        LocalizationManager.Instance.WasLanguageChanged = false;
        while (count < CategoryStates.Count)
        {          
            count = 0;
            for (int i = 0; i < CategoryStates.Count; i++)
            {
                if (CategoryStates[i].loadedSprites)
                {
                    count++;
                }
            }
           
            timer += checkingInterval;          
            if (timer >= TimeOut && !m_isTimeOut)
            {
                TimeOutScreen();
            }
            yield return WFSChecking;

        }
        SwitchLoadingScreen(false);
        SaveCategory (LocalizationManager.Instance.secondLanguage);              
    }

    void TimeOutScreen()
    {
        StopAllCoroutines();
        m_isTimeOut = true;
        m_audio.Play();
        m_popupOpener.popupPrefab = LostConnectionPopup;
        m_popupOpener.OpenPopup();
    }

    public void RestartConnection()
    {
        m_isTimeOut = false;
        InitializeBundlesLoad(CategoryStates);
        StartCoroutine(CheckIfAllLoaded());
    }

    public void DontRestart()
    {
        m_isTimeOut = false;
        StopAllCoroutines();
        SwitchLoadingScreen(false);
        Category[] categories = GameObject.Find("Canvas")
            .transform.Find("Categories").GetComponentsInChildren<Category>();
 
        for (int i = 0; i < categories.Length; i++)
        {           
            if (!FindCategory(categories[i].CategoryName).loadedSprites)
            {
                categories[i].Inactivate();
            }           
        }        
    }

    public void Load(string name)
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf1 = new BinaryFormatter();
            FileStream file1 = File.Open(Application.persistentDataPath
                + "/playerData.dat", FileMode.Open);
            PlayerData playerData = (PlayerData)bf1.Deserialize(file1);
            avaliableCategories = playerData.avaliableCategories;
            inventory = playerData.inventory;
            secondLaunch = playerData.secondLaunch;
            firstLanguage = playerData.firstLanguage;
            secondLanguage = playerData.secondLanguage;
            m_languegeCount = playerData.languageCount;
            LocalizationManager.Instance.LoadUITexts("UI/UI" + firstLanguage);
            file1.Close();

            if (m_languegeCount < languages.Count)
            {
                
                for (int i = m_languegeCount; i < languages.Count; i++)
                {
                    CategoryStates = new List<CategoryState>();
                   
                    AddNewLanguagesToBaseCategories(languages[i]);
                    SaveCategory(languages[i]);

                }
                m_languegeCount = languages.Count;
                SavePlayerData();
            }
        }

        if (File.Exists(Application.persistentDataPath + "/" + name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath
                + "/" + name + ".dat", FileMode.Open);
            States categoryStates = (States)bf.Deserialize(file);
            CategoryStates = categoryStates.categoryStates;
            file.Close();         
        }
       
        else
        {
            CreatePlayerData();
        }       
    }

    void AddNewLanguagesToBaseCategories(string language)
    {
        for (int i = 0; i < m_baseCategoriesCount; i++)
        {
            if (!avaliableCategories[i].avaliableLanguages.Contains(language))
            {
                avaliableCategories[i].avaliableLanguages.Add(language);
            }
        }
    }

    public IEnumerator LoadFirstAssetBundle(CategoryState categoryState)
    {
        string filePath;
        if (categoryState.isBasic)
        {           
            if (Application.platform == RuntimePlatform.Android)
            {
                filePath = "jar:file://" + Application.dataPath + "!/assets/" + categoryState.categoryName;
            }
            else
            {              
                filePath = Application.dataPath + "/StreamingAssets/" + categoryState.categoryName; 
            }
            AssetBundle bundle = AssetBundle.LoadFromFile(filePath);
            Unload(bundle);
            categoryState.loadedSprites = true;
            SaveCategory(LocalizationManager.Instance.secondLanguage);          
        }

        else
        {         
            filePath = categoryState.url;
            WWW www = WWW.LoadFromCacheOrDownload(filePath, categoryState.num);

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
            }
            else
            {
                AssetBundle bundle = www.assetBundle;
                Unload(bundle);
                categoryState.loadedSprites = true;
                SaveCategory(LocalizationManager.Instance.secondLanguage);
            }
        }   
    }

    void Unload(AssetBundle bundle)
    {
        bundle.Unload(true);
        bundle = null;
    }

    void CreatePlayerData()
    {     
        SetFirstLanguage(Application.systemLanguage);      
        avaliableCategories = new List<CategoryAvaliability>();
        inventory = new List<POWERUP>();       
        secondLaunch = false;
     
        m_languegeCount = languages.Count;
        SavePlayerData();
        for (int i = 0; i < languages.Count; i++)
        {
            CategoryStates = new List<CategoryState>();       
            SaveCategory(languages[i]);
        }                
    }

    void SetFirstLanguage(SystemLanguage systemLanguage)
    {
        switch (systemLanguage)
        {
            case SystemLanguage.English:
                firstLanguage = "english";
                secondLanguage = "english";
                LocalizationManager.Instance.LoadUITexts("UI/UIenglish");
                break;
            case SystemLanguage.German:
                firstLanguage = "german";
                secondLanguage = "german";
                LocalizationManager.Instance.LoadUITexts("UI/UIgerman");
                break;
            case SystemLanguage.Polish:
                firstLanguage = "polish";
                secondLanguage = "polish";
                LocalizationManager.Instance.LoadUITexts("UI/UIpolish");
                break;
            case SystemLanguage.Spanish:
                firstLanguage = "spanish";
                secondLanguage = "spanish";
                LocalizationManager.Instance.LoadUITexts("UI/UIspanish");
                break;
            case SystemLanguage.French:
                firstLanguage = "french";
                secondLanguage = "french";
                LocalizationManager.Instance.LoadUITexts("UI/UIfrench");
                break;
            default:
                firstLanguage = "english";
                secondLanguage = "english";
                LocalizationManager.Instance.LoadUITexts("UI/UIenglish");
                break;

        }
    }

    public void NewGame()
    {
        Caching.ClearCache();  
        File.Delete(Application.persistentDataPath + "/playerData.dat");
        for (int i = 0; i < languages.Count; i++)
        {
            File.Delete(Application.persistentDataPath + "/" + languages[i] + ".dat");
        }            
        FindObjectOfType<LivesManager>().FillLives();
        PlayerPrefs.DeleteAll();
    }

    public void TakeWheeleItem()
    {
        if(SelectedWheelItem != null)
        {
            var wheelItem = SelectedWheelItem.GetComponent<WheelIten>();
            int count = wheelItem.count;
            for (int i = 0; i < count; i++)
            {
                inventory.Add(wheelItem.powerUp);
            }          
        }       
        SavePlayerData();
    }

    public void AddCategory(string name, string url, bool isBasic)
    {      
        int num = CategoryStates.Count;
        bool loadedSprites = false;
        int[] levelStates = new int[12] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
     
        int[] scores = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        CategoryState categoryState = new CategoryState
            (levelStates, scores, name, loadedSprites, url, num, isBasic);
        CategoryStates.Add(categoryState);     
    }  

    public CategoryState FindCategory(string name)
    {
        for (int i = 0; i < CategoryStates.Count; i++)
        {
            if(CategoryStates[i].categoryName == name)
            {
                return CategoryStates[i];
            }           
        }     
        return null;
    }  
}
