using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPopup : MonoBehaviour {

    public Image Image;

    public Text Name;

    

    public void PlayAd()
    {
        AdManager.manager.ShowAd();
    }

    private void Awake()
    {
        Image.sprite = AdManager.manager.boosterToAdd.Icon;
        Name.text = LocalizationManager.Instance.GetUIValue(AdManager.manager.boosterToAdd.gameObject.name);
    }

}
