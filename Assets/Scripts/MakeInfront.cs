using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInfront : MonoBehaviour {

    private GameManager m_gameManager;
    public Text text;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
    }

    public void SetLast()
    {
        transform.SetAsLastSibling();
    }

    private void Update()
    {
        if(m_gameManager.GameState != GAME_STATE.PLAY)
        {
            text.text = "Translation and\npronunciation features!";
        }
        else
        {
            text.text = "Fun and challenging\ngameplay!";
        }
    }
}
