using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslationText : MonoBehaviour {
  
    Animator m_anim;

    Text m_text;

    string translatedWord;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_anim = GetComponent<Animator>();
        m_text = GetComponentInChildren<Text>();
    }

    public void Translate(string word)
    {      
        translatedWord = word;
        m_anim.Play("Translate", -1, 0f);
    }

    public void SetText()
    {
        m_text.text = translatedWord;
    }
}
