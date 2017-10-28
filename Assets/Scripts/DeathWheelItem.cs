using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathWheelItem : WheelIten {

    private Text text;

    private GameManager m_gameManager;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        
        m_gameManager = FindObjectOfType<GameManager>();
        if(m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
        {
            count = count / 2;
        }
        if (powerUp == POWERUP.DEATH)
            return;
        text.text = count.ToString("0");
    }
}
