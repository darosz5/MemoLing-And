using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class DemoManager : MonoBehaviour {

    private GameManager m_gameManager;
    private PopupOpener m_popupOpener;

    public GameObject LearnPopup;
    public GameObject MatchPopup;
    public GameObject WinPopup;

    private void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void InstantiateDemo()
    {

        string originalFileName = "demo/" + LocalizationManager.Instance.firstLanguage;
        string localizedFileName = "demo/" + LocalizationManager.Instance.secondLanguage;
        
        LocalizationManager.Instance.LoadOriginalText(originalFileName);
        LocalizationManager.Instance.LoadLocalizedText(localizedFileName);
        List<string> keys = new List<string>(LocalizationManager.Instance.localizedText.Keys);
        LevelsManager.Instance.numOfStages = 2;
        StartCoroutine(m_gameManager.InstantiateGridCO(keys, 300f, 6));
    }

    public void OpenPopup(GameObject popup)
    {
        m_popupOpener.popupPrefab = popup;
        m_popupOpener.OpenPopup();
    }
}
