using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelItemPopup : MonoBehaviour {

    [Header("References")]

    public Image Image;

    public Text Text;

    void Awake()
    {
        GameObject selectedItem =  GameControl.control.SelectedWheelItem;
        Image.sprite = selectedItem.GetComponentInChildren<SpriteRenderer>().sprite;
        Text.text = LocalizationManager.Instance.GetUIValue(selectedItem.name);
    }
}
