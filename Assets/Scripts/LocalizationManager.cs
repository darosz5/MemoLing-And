using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public Dictionary<string, string> localizedText;
    public Dictionary<string, string> originalText;
    public Dictionary<string, string> UITexts;
 
    public string firstLanguage;
    public string secondLanguage;

    public string category;
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
   
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    private static LocalizationManager instance = null;
    public static LocalizationManager Instance
    {
        get { return instance; }
        set { }

    }
    void Awake()
    {
        WasLanguageChanged = true;
        
       
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
        
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        firstLanguage = GameControl.control.firstLanguage;
        secondLanguage = GameControl.control.secondLanguage;
    }

   

    public void SetCategory(string name)
    {
       LocalizationManager.instance.category = name;
        category = LocalizationManager.instance.category;
       
    }

    public void SetFirstLauguageName(string name)
    {
        LocalizationManager.instance.firstLanguage = name;
        firstLanguage = LocalizationManager.instance.firstLanguage;
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
        LocalizationManager.instance.secondLanguage = name;
        secondLanguage = LocalizationManager.instance.secondLanguage;
        
        GameControl.control.secondLanguage = secondLanguage;
        LocalizationManager.instance.WasLanguageChanged = true;
        FindObjectOfType<LangaugeFlags>().SetFlags();
        PlayerPrefs.SetString("language", LocalizationManager.instance.secondLanguage);
        LocalizationManager.Instance.LoadUITexts("UI/UI" + firstLanguage);
        GameControl.control.SavePlayerData();
        GameControl.control.Load(LocalizationManager.instance.secondLanguage);        
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