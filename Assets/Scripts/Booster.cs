using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour {

    [Header("References")]

    public Sprite Icon;

    public GameObject CheckBox;

    public Text AmountText;

    public GameObject AddButton;

    [Header("Varibles")]

    public POWERUP PowerUp;

    int m_amount;
   
    void Awake()
    {
         UpdateBooster(true);
    }

    public void SetBooster()
    {
        AdManager.manager.boosterToAdd = this;      
        AdManager.manager.powerup = PowerUp;
    }

    public void UpdateBooster(bool checkmarked)
    {
        m_amount = 0;
        for (int i = 0; i < GameControl.control.inventory.Count; i++)
        {
            if (GameControl.control.inventory[i] == PowerUp)
            {
                m_amount++;
            }
        }

        if (m_amount > 0)
        {
            AddButton.gameObject.SetActive(false);
            AmountText.transform.parent.gameObject.SetActive(true);
            AmountText.text = m_amount.ToString();
            CheckBox.gameObject.SetActive(checkmarked);          
        }

        else if (m_amount == 0)
        {
            AddButton.gameObject.SetActive(true);
            AmountText.transform.parent.gameObject.SetActive(false);
            CheckBox.gameObject.SetActive(false);
        }
    }   
}
