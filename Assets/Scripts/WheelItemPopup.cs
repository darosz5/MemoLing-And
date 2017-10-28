using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelItemPopup : MonoBehaviour {

    public Image Image;

    public Text Text;

    private void Awake()
    {
        GameObject selectedItem =  GameControl.control.SelectedWheelItem;

        Image.sprite = selectedItem.GetComponentInChildren<SpriteRenderer>().sprite;
        Text.text = LocalizationManager.Instance.GetUIValue(selectedItem.name);
    }
}
