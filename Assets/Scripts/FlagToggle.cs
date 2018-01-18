using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagToggle : MonoBehaviour {

    void Awake()
    {
        if(gameObject.name == LocalizationManager.Instance.secondLanguage)
        {
            GetComponent<Toggle>().isOn = true;
            IAPManager.Instance.language = gameObject.name;
        }
    }

    public void StuLanguage()
    {
        IAPManager.Instance.language = gameObject.name;
    }
}
