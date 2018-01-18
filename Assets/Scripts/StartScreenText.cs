using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenText : MonoBehaviour {

    public string key;

    Text m_text;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        m_text = GetComponent<Text>();
        if (!string.IsNullOrEmpty(LocalizationManager.Instance.firstLanguage))
        {
            m_text.text = LocalizationManager.Instance.GetUIValue(key);
        }
    }

    public void SetLanguage()
    {
        m_text.text = LocalizationManager.Instance.GetUIValue(key);
    }
}
