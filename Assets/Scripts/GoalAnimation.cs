using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalAnimation : MonoBehaviour {

    private GameManager m_gameManager;
    private ScoreManager m_scoreManager;
    private AudioSource m_audio;

    public Text AmountText;
    

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_audio = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        m_scoreManager.CountersOff();
        AmountText.text = LocalizationManager.Instance.GetUIValue("_finishin");
        if(m_gameManager.StageGoal.goal == GAME_GOAL.TIME)
        {
            AmountText.text += (m_gameManager.StageGoal.amount.ToString() + " " +
                LocalizationManager.Instance.GetUIValue("_seconds"));
        }
        else if (m_gameManager.StageGoal.goal == GAME_GOAL.MOVES)
        {
            AmountText.text += (m_gameManager.StageGoal.amount.ToString() + " " +
                LocalizationManager.Instance.GetUIValue("_moves"));
        }
        
    }

    public void StartStage()
    {
              
        transform.parent.gameObject.SetActive(false);

    }

    public void PlaySoundEffect()
    {
        m_audio.Play();
    }
}
