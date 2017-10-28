using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour {

    public POWERUP PowerUp;
    public Sprite Icon;
    public GameObject CheckBox;
    public Text AmountText;
    public GameObject AddButton;
    

    private int amount;
   

    private void Awake()
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
        amount = 0;
        for (int i = 0; i < GameControl.control.inventory.Count; i++)
        {
            if (GameControl.control.inventory[i] == PowerUp)
            {
                amount++;
            }
        }
        if (amount > 0)
        {
            AddButton.gameObject.SetActive(false);
            AmountText.transform.parent.gameObject.SetActive(true);
            AmountText.text = amount.ToString();
            CheckBox.gameObject.SetActive(checkmarked);
            

        }
        else if (amount == 0)
        {
            AddButton.gameObject.SetActive(true);
            AmountText.transform.parent.gameObject.SetActive(false);
            CheckBox.gameObject.SetActive(false);

        }
    }

   
}
