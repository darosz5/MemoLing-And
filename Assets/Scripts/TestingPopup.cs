using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingPopup : MonoBehaviour {

    public InputField NumOfCardsInputField;
    
    public void SetModifiers()
    {      
        LevelsManager.Instance.numOfCards = int.Parse(NumOfCardsInputField.text);            
        //GameControl.control.IsTesting = true;
    }
}
