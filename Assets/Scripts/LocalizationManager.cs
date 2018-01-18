using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public Dictionary<string, string> localizedText;

    public Dictionary<string, string> originalText;

    public Dictionary<string, string> UITexts;
 
    [HideInInspector]
    public string firstLanguage;

    [HideInInspector]
    public string secondLanguage;

    [HideInInspector]
    public string category;

    [HideInInspector]
    public bool WasLanguageChanged;

    public readonly List<string> LongWords = new List<string>()
    {
        "el calentamiento\nglobal",
        "le réchauffement\nde la planète",
        "die\nWiederverwertung",
        "die\nKrankenschwester",
        "okulary\nprzeciwsłoneczne",
        "der\nSonnenuntergang",
        "Schlittschuhlaufen",
        "der\nBaseballhandschuh",
        "Fallschirmspringen",
        "das\nFahrradergometer",
        "el edificio\nde apartamentos",
        "apartamentowiec",
        "das\nAppartementhaus",
        "der\nFußgängerübergang",
        "der\nVerkaufsautomat",
        "der\nKapuzenpullover",
        "Fausthandschuhe",
        "die\nDamenunterwäsche",
        "niepełnosprawny",
        "einen Heiratsantrag\nmachen",
        "die Vereinigten\nArabischen Emirate",
        "Zjednoczone\nEmiraty Arabskie",
        "faire une demande\nen mariage",
        "das\nEinkaufszentrum",
        "die\nStraßenlaterne"
    };
   
    bool isReady = false;

    string missingTextString = "Localized text not found";


    void Awake()
    {
        WasLanguageChanged = true;
        MakePersistentSingleton();
    }

    void MakePersistentSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        firstLanguage = GameControl.control.firstLanguage;
        secondLanguage = GameControl.control.secondLanguage;
    }

    public void SetCategory(string name)
    {
        LocalizationManager.Instance.category = name;
        category = LocalizationManager.Instance.category;      
    }

    public void SetFirstLauguageName(string name)
    {
        LocalizationManager.Instance.firstLanguage = name;
        firstLanguage = LocalizationManager.Instance.firstLanguage;
        GameControl.control.firstLanguage = name;
        
        
        LocalizationManager.Instance.LoadUITexts("UI/UI" + firstLanguage);
        StartScreenText[] startScreenTexts = GameObject.FindObjectsOfType<StartScreenText>();
        for (int i = 0; i < startScreenTexts.Length; i++)
        {
            startScreenTexts[i].SetLanguage();
        }
    }

    public void SetSecondLauguageName(string name)
    {
        LocalizationManager.Instance.secondLanguage = name;
        secondLanguage = LocalizationManager.Instance.secondLanguage;
        
        GameControl.control.secondLanguage = secondLanguage;
        LocalizationManager.Instance.WasLanguageChanged = true;
        FindObjectOfType<LangaugeFlags>().SetFlags();
        PlayerPrefs.SetString("language", LocalizationManager.Instance.secondLanguage);
        LocalizationManager.Instance.LoadUITexts("UI/UI" + firstLanguage);
        GameControl.control.SavePlayerData();
        GameControl.control.Load(LocalizationManager.Instance.secondLanguage);        
    }

    public void LoadLocalizedText(string fileName)
    {     
        localizedText = new Dictionary<string, string>();

        TextAsset dataAsJson = Resources.Load<TextAsset>(fileName);

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson.text);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        
        isReady = true;
    }

    public void LoadOriginalText(string fileName)
    {      
        originalText = new Dictionary<string, string>();

        TextAsset dataAsJson = Resources.Load<TextAsset>(fileName);
        
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson.text);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            originalText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
       
        isReady = true;
    }

    public void LoadUITexts(string fileName)
    {
        UITexts = new Dictionary<string, string>();

        TextAsset dataAsJson = Resources.Load<TextAsset>(fileName);

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson.text);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            UITexts.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public string GetOriginalValue(string key)
    {
        string result = missingTextString;
        if (originalText.ContainsKey(key))
        {
            result = originalText[key];
        }
        return result;
    }

    public string GetUIValue(string key)
    {
        string result = missingTextString;
        if (UITexts.ContainsKey(key))
        {
            result = UITexts[key];
        }
        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }
}