using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;
using ExaGames.Common;

public class StartLevelButton : MonoBehaviour {

    [Header("References")]

    public Booster[] boosters;

    LivesManager m_liverManager;

    SceneTransition m_sceneTransition;

    PopupOpener m_popupOpener;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_liverManager = FindObjectOfType<LivesManager>();
        m_sceneTransition = FindObjectOfType<SceneTransition>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void SetBoosters()
    {
        GameControl.control.activeBoosters.Clear();
        for (int i = 0; i < boosters.Length; i++)
        {
            if (boosters[i].CheckBox.GetComponent<Toggle>().isOn
                && boosters[i].CheckBox.activeInHierarchy)
            {
                GameControl.control.activeBoosters.Add(boosters[i]);
            }
        }
    }

    public void StartLevel()
    {
        if(m_liverManager.CanPlay)
        {
            SetBoosters();
            m_sceneTransition.PerformTransition();          
        }

        else
        {
            m_popupOpener.OpenPopup();
        }
    }
}
