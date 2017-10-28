using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenText : MonoBehaviour {

    private Text text;

    public string key;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        text = GetComponent<Text>();
        if (!string.IsNullOrEmpty(LocalizationManager.Instance.firstLanguage))
        {
            text.text = LocalizationManager.Instance.GetUIValue(key);
        }
    }

    public  void SetLanguage()
    {
        text.text = LocalizationManager.Instance.GetUIValue(key);
    }
}
