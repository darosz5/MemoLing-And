using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddLivesPopup : MonoBehaviour {

    [Header("Variables")]

    public POWERUP powerup;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay" &&
            FindObjectOfType<GameManager>().GameState == GAME_STATE.LOSE)
        {          
            transform.SetParent(GameObject.Find("Canvas Lose").transform, false);
        }
    }

    public void AddLiveWithVideo()
    {
        AdManager.manager.powerup = powerup;
        AdManager.manager.ShowAd();       
    }
}
