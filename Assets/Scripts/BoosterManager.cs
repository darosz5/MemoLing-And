using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;
using System;

public class BoosterManager : MonoBehaviour {


    [Header("Booster buttons")]
    public GameObject KillerBooster;
    public GameObject TimeBooster;
    public GameObject SideBooster;

    private GameManager m_gameManager;

    public List<GameObject> ActiveButtons;

    public Color[] colors;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        DisableButtons();
    }

    public void DisableButtons()
    {
        DisableButton(KillerBooster);
        DisableButton(SideBooster);
        DisableButton(TimeBooster);
    }

  


    public void HandleBoosters()
    {

        for (int i = 0; i < GameControl.control.activeBoosters.Count; i++)
        {
            switch (GameControl.control.activeBoosters[i].PowerUp)
            {
                case POWERUP.KILLER:                
                    ActiveButtons.Add(KillerBooster);                 
                    break;
                case POWERUP.SHOW_SIDE:                  
                    ActiveButtons.Add(SideBooster);
                    break;
                case POWERUP.ADDITIONAL_TIME:
                    ActiveButtons.Add(TimeBooster);                 
                    break;

            }
        }
       
    }

    public void ToogleKiller()
    {
        Image img = KillerBooster.GetComponentsInChildren<Image>()[1];
        if (img.color == colors[0])
        {
            img.color = colors[1];
            m_gameManager.ShowPairb = true;
        }
        else
        {
            img.color = colors[0];
            m_gameManager.ShowPairb = false;
        }
    }
    public void ToogleSide()
    {

        Image img = SideBooster.GetComponentsInChildren<Image>()[1];
        if (img.color == colors[0])
        {
            img.color = colors[1];
            m_gameManager.ShowSideb = true;
        }
        else
        {
            img.color = colors[0];
            m_gameManager.ShowSideb = false;
        }
    }
    public void ToogleTime()
    {

        Image img = TimeBooster.GetComponentsInChildren<Image>()[1];
        if (img.color == colors[0])
        {
            img.color = colors[1];
            m_gameManager.MoreTimeOrMovesb = true;
        }
        else
        {
            img.color = colors[0];
            m_gameManager.MoreTimeOrMovesb = false;
        }
    }

    public void EnableButton(GameObject button)
    {
        var image = button.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 1.0f;
        image.color = newColor;
        image.color = colors[0];

        var shadow = button.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 1.0f;
        shadow.color = newShadowColor;

        var buttoning = button.GetComponentsInChildren<Image>()[2];
        var color = buttoning.color;
        color.a = 1.0f;
        buttoning.color = color;

        button.GetComponent<AnimatedButton>().interactable = true;
    }

    public void DisableButton(GameObject button)
    {
        var image = button.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;

        var shadow = button.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 40 / 255.0f;
        shadow.color = newShadowColor;

        var butonimg = button.GetComponentsInChildren<Image>()[2];
        var color = butonimg.color;
        color.a = 40 / 255.0f;
        butonimg.color = color;

        button.GetComponent<AnimatedButton>().interactable = false;
    }

    public void EnableActiveButtons()
    {
        for (int i = 0; i < ActiveButtons.Count; i++)
        {
            EnableButton(ActiveButtons[i]);
        }
    }


    
}
