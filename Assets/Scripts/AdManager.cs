using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Ricimi;
using UnityEngine.UI;
using ExaGames.Common;

public class AdManager : MonoBehaviour {

    public static AdManager manager;

    [Header("References")]

    public GameObject SkippedAdPopup;

    public GameObject FailedAdPopup;

    PopupOpener m_popupOpener;

    [HideInInspector]
    public Booster boosterToAdd;

    [HideInInspector]
    public POWERUP powerup;

    void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
        MakePersistentSingleton();
    }

    void MakePersistentSingleton()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Advertisement.Initialize("1366024", false);
    }

    public void ShowAd()
    {
        
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhandler;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            m_popupOpener.popupPrefab = FailedAdPopup;
            m_popupOpener.OpenPopup();
            return;
        }
        float startTime = Time.timeSinceLevelLoad ;
        GameControl.control.SwitchLoadingScreen(true);
        if (!Advertisement.isInitialized)
        {
            
            Advertisement.Initialize(Advertisement.gameId);
            while (!Advertisement.isInitialized)
            {               
                if (Time.timeSinceLevelLoad - startTime > 10f)
                    break;
            }
            
        }
        startTime = Time.timeSinceLevelLoad;

        while (!Advertisement.IsReady())
        {
            if (Time.timeSinceLevelLoad - startTime > 10f)
                break;
        }
        Advertisement.Show(options);
        


        GameControl.control.SwitchLoadingScreen(false);
    }

    void AdCallbackhandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:

                if(powerup == POWERUP.LIVE)
                {             
                    FindObjectOfType<LivesManager>().GiveOneLife();                                
                }
                else if(powerup == POWERUP.TIME_AD)
                {
                    GameControl.FindObjectOfType<GameManager>().
                        AddTimeOrMoves(GameControl.control.deathWheelItem.count);
                }

                else if (powerup == POWERUP.SHOW_SIDE || powerup == POWERUP.KILLER || powerup == POWERUP.ADDITIONAL_TIME)
                {
                    GameControl.control.inventory.Add(boosterToAdd.PowerUp);

                    boosterToAdd.UpdateBooster(true);
                    boosterToAdd.CheckBox.GetComponent<Toggle>().isOn = true;
                    GameControl.control.SavePlayerData();
                }
              
                else
                {
                    Debug.Log(powerup);
                }
                  
                break;
            case ShowResult.Skipped:
                m_popupOpener.popupPrefab = SkippedAdPopup;
                m_popupOpener.OpenPopup();
                break;
            case ShowResult.Failed:
                m_popupOpener.popupPrefab = FailedAdPopup;
                m_popupOpener.OpenPopup();
                break;
        }
    }

   
}
