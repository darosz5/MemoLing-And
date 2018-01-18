using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathWheelItem : WheelIten {

    Text m_text;

    GameManager m_gameManager;

    void Awake()
    {
        GetReferences();
        Initialize();
    }

    void GetReferences()
    {
        m_text = GetComponentInChildren<Text>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void Initialize()
    {
        if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
        {
            count = count / 2;
        }
        if (powerUp == POWERUP.DEATH)
            return;
        m_text.text = count.ToString("0");
    }
}
