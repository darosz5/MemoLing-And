using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPModul : MonoBehaviour {

    [Header("References")]

    public Text MessageText;

    void Awake()
    {
        MessageText.text = LocalizationManager.Instance.GetUIValue("_welcome");
    }

    public void BuyCategory(string category)
    {
        string productID = IAPManager.Instance.language + "_" + category;
        IAPManager.Instance.BuyProductID(productID);     
    }
   
    public void HandleMessage(int i)
    {
        if(i == 0)
        {
            MessageText.text = LocalizationManager.Instance.GetUIValue("_shopsuccess");
        }
        else if (i == 1)
        {
            MessageText.text = LocalizationManager.Instance.GetUIValue("_shopfail");
        }
        else if (i == 2)
        {
            MessageText.text = LocalizationManager.Instance.GetUIValue("_shopalreadyhas");
        }
    }
}
