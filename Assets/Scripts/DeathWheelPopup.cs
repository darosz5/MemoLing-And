using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;

public class DeathWheelPopup : MonoBehaviour {

    [Header("References")]

    public Image Image;

    public Text Text;

    public AnimatedButton Button;

    GameManager m_gameManager;

    
    void Awake()
    {
        m_gameManager = GameManager.FindObjectOfType<GameManager>();
        Initialize();
    }

    void Initialize()
    {
        GameObject selectedItem = GameControl.control.SelectedWheelItem;

        Image.sprite = selectedItem.GetComponentInChildren<SpriteRenderer>().sprite;

        if (GameControl.control.deathWheelItem.powerUp == POWERUP.DEATH)
        {
            Destroy(gameObject);
            m_gameManager.WasTimeAdded = true;
            m_gameManager.GameOver();
        }

        if (GameControl.control.deathWheelItem.powerUp == POWERUP.TIME_AD ||
            GameControl.control.deathWheelItem.powerUp == POWERUP.TIME)
        {
            Text.text = (GameControl.control.deathWheelItem.count).ToString();
            if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
            {
                Text.text += " " + LocalizationManager.Instance.GetUIValue("_moves");
            }
            else if (m_gameManager.StageGoal.goal == GAME_GOAL.TIME)
            {
                Text.text += " " + LocalizationManager.Instance.GetUIValue("_seconds");
            }
            if (GameControl.control.deathWheelItem.powerUp == POWERUP.TIME_AD)
            {
                Text.text += "\n" + LocalizationManager.Instance.GetUIValue("_withvideo");

            }
        }
    }

    public void RecieveReward()
    {
        if(GameControl.control.deathWheelItem.powerUp == POWERUP.TIME)
        {
            m_gameManager.AddTimeOrMoves(GameControl.control.deathWheelItem.count);
        }
        if (GameControl.control.deathWheelItem.powerUp == POWERUP.TIME_AD)
        {
            AdManager.manager.powerup = GameControl.control.deathWheelItem.powerUp;
            AdManager.manager.ShowAd();
        }
    }

    public void GameOver()
    {
        m_gameManager.WasTimeAdded = true;
        m_gameManager.GameOver();
    }
}
