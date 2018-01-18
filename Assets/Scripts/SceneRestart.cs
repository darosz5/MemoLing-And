using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using ExaGames.Common;
using UnityEngine.SceneManagement;

public class SceneRestart : MonoBehaviour {

    LivesManager m_liverManager;

    PopupOpener m_popupOpener;

    GameManager m_gameManager;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_liverManager = FindObjectOfType<LivesManager>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void RestartScene()
    {       
        StartCoroutine(RestartSceneCO());
    }
    
    IEnumerator RestartSceneCO()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartSceneWithLives()
    {
        if (m_liverManager.CanPlay)
        {
            if (!GameControl.control.WasLevelWon && !m_gameManager.WasTimeAdded)
            {
                m_liverManager.ConsumeLife();
            }
            RestartScene();
        }
        else
        {
            m_popupOpener.OpenPopup();
        }            
    }
}
