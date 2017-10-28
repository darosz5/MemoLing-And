using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdButton : MonoBehaviour {

    public GameObject GameOverText;

    private GameManager m_gameManager;

    private void OnEnable()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        if (m_gameManager)
        {
            if (m_gameManager.WasTimeAdded)
            {
                GameOverText.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                GameOverText.SetActive(false);
                gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("No game manager");
        }
    }
}
