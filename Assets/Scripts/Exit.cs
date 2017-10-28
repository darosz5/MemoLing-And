using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using ExaGames.Common;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    private SceneTransition m_sceneTransition;

    private void Awake()
    {
        m_sceneTransition = GetComponent<SceneTransition>();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitGameplay()
    {
        
        if (GameControl.control.IsPlayingDemo)
        {
            m_sceneTransition.scene = "HomeScene 1";
            m_sceneTransition.PerformTransition();
        }
        else
        {
            GameObject.FindObjectOfType<LivesManager>().ConsumeLife();
            m_sceneTransition.PerformTransition();
        }
        GameControl.control.IsPlayingDemo = false;
    }

    public void PlayAd()
    {
        AdManager.manager.ShowAd();
    }

    public void SkippedAdGameplay()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Gameplay"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.WasTimeAdded = true;
            gameManager.GameOver();
        }
    }

    
}
