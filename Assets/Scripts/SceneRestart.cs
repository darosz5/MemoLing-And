using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using ExaGames.Common;
using UnityEngine.SceneManagement;

public class SceneRestart : MonoBehaviour {

    private LivesManager m_liverManager;

    private PopupOpener m_popupOpener;

    private GameManager m_gameManager;

    private void Awake()
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
