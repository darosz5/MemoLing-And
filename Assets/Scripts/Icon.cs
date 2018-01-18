using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Icon : MonoBehaviour, IPointerDownHandler
{

    [Header("References")]

    public Image image;

    public Text text;
   
    public GameObject RevealedContent;

    GameManager m_gameManager;

    GameObject m_translationText;

    Animator m_anim;

    AudioSource m_audio;

    [HideInInspector]
    public string Key;

    [HideInInspector]
    public bool IsIcon;

    [HideInInspector]
    public bool IsMatched;

    bool m_isRevealed;

    string m_textToSpeak;

    void Awake()
    {
        GetReferences();
        m_audio.Play();
    }

    void GetReferences()
    {
        m_anim = GetComponent<Animator>();
        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        m_audio = GetComponent<AudioSource>();
        m_translationText = GameObject.FindGameObjectWithTag("Translation");
    }

    void SetTextToSpeek()
    {
        m_textToSpeak = text.text.Replace("-\n", string.Empty);
        m_textToSpeak = m_textToSpeak.Replace("\n", " ");
    }    

    public void Reveal()
    {
        m_anim.SetBool("Revealed_b", true);
        if (m_gameManager.FirstSelection)
        {
            m_audio.Play();
        }       
        m_isRevealed = true;
    }

    public void RevelAtStart()
    {
        m_anim.SetBool("Revealed_b", true);
        if (IsIcon)
        {
            m_audio.Play();
        }
    }

    public void Hide()
    {
        m_anim.SetBool("IsPlayState_b", m_gameManager.GameState == GAME_STATE.PLAY);
        m_anim.SetBool("Revealed_b", false);
        m_isRevealed = false;      
    }

    public void HideAtStart()
    {
        m_anim.SetTrigger("Hide_t");
    }

    public void Initialize(string key, string text, Sprite sprite = null)
    {
        this.text.text = text;
        if (LocalizationManager.Instance.LongWords.Contains(text))
        {
            this.text.fontSize = 25;
        }
        Key = key;  
        if(sprite != null)
        {
            image.sprite = sprite;
        }
        if (!IsIcon)
        {
            SetTextToSpeek();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameControl.control.IsChecking)
        {
            m_gameManager.NextCheck();
        }


        if (m_gameManager.GameState == GAME_STATE.SHUFFLING)
            return;

        else if(m_gameManager.GameState == GAME_STATE.INIT)
        {
            if (IsIcon)
                return;
            else
            {
                Speak(m_textToSpeak);
            }
            
        }

        else if ((m_isRevealed && m_gameManager.GameState == GAME_STATE.PLAY) ||
            m_gameManager.GameState == GAME_STATE.LEARN || IsMatched)
        {
            if (IsIcon)
            {
                m_translationText.GetComponent<TranslationText>().Translate(this.text.text);
                m_audio.Play();
            }
            else
            {
                Speak(m_textToSpeak);

                
            }
        }
        else
        {
            m_gameManager.SelectButton(this);
        }
   }

    void Speak(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            TTSManager.Speak(text, false);
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            EasyTTSUtil.SpeechAdd(text);
        }
    }

    public void Highlight(bool match)
    {
        if (match)
        {
            m_anim.SetTrigger("Greenhighlight_t");
        }
        else
        {
            m_anim.SetTrigger("Redhighlight_t");
        }
    }
}
