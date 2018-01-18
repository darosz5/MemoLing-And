using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakTest : MonoBehaviour {

    Text m_text;

    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        m_text.text = TTSManager.IsSpeaking().ToString();
    }
}
