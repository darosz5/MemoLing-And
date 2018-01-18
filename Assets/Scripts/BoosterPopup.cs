using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPopup : MonoBehaviour {

    [Header("References")]

    public Image Image;

    public Text Name;    

    public void PlayAd()
    {
        AdManager.manager.ShowAd();
    }

    void Awake()
    {
        Image.sprite = AdManager.manager.boosterToAdd.Icon;
        Name.text = LocalizationManager.Instance.GetUIValue(AdManager.manager.boosterToAdd.gameObject.name);
    }

}
