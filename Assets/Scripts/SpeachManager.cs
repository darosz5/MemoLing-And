using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeachManager : MonoBehaviour {

    public static SpeachManager speachManager;

    private void Awake()
    {
        
        if (speachManager == null)
        {
            DontDestroyOnLoad(gameObject);
            speachManager = this;
        }
        else if (speachManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
         TTSManager.Initialize(transform.name, "OnInit");
       // if(LocalizationManager.Instance.secondLanguage != null)
       // {
          //  SetSpeach();
       // }          
    }

    public void SetSpeach()
    {
        
        switch (LocalizationManager.Instance.secondLanguage)
        {
            case "spanish":
                if(Application.platform == RuntimePlatform.Android)
                {
                    if (TTSManager.IsLanguageAvailable(TTSManager.SPANISH))
                    {
                        TTSManager.SetLanguage(TTSManager.SPANISH);
                        GameControl.control.isLanguageAvalibleForSpeech = true;
                    }
                }
                if(Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    EasyTTSUtil.Initialize(EasyTTSUtil.Spain);
                    GameControl.control.isLanguageAvalibleForSpeech = true;
                }
                break;
            case "polish":
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (TTSManager.IsLanguageAvailable(TTSManager.POLISH))
                    {
                        TTSManager.SetLanguage(TTSManager.POLISH);
                        GameControl.control.isLanguageAvalibleForSpeech = true;
                    }
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    EasyTTSUtil.Initialize(EasyTTSUtil.Poland);
                    GameControl.control.isLanguageAvalibleForSpeech = true;
                }
                break;

            case "french":
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (TTSManager.IsLanguageAvailable(TTSManager.FRENCH))
                    {
                        TTSManager.SetLanguage(TTSManager.FRENCH);
                        GameControl.control.isLanguageAvalibleForSpeech = true;
                    }
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    EasyTTSUtil.Initialize(EasyTTSUtil.France);
                    GameControl.control.isLanguageAvalibleForSpeech = true;
                }            
                break;

            case "english":
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (TTSManager.IsLanguageAvailable(TTSManager.ENGLISH))
                    {
                        TTSManager.SetLanguage(TTSManager.ENGLISH);
                        GameControl.control.isLanguageAvalibleForSpeech = true;
                    }
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    EasyTTSUtil.Initialize(EasyTTSUtil.France);
                }
                break;

            case "german":
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (TTSManager.IsLanguageAvailable(TTSManager.GERMAN))
                    {
                        TTSManager.SetLanguage(TTSManager.GERMAN);
                        GameControl.control.isLanguageAvalibleForSpeech = true;
                    }
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    EasyTTSUtil.Initialize(EasyTTSUtil.Germany);
                }
                break;

            default:
                //Debug.Log("No language");
                GameControl.control.isLanguageAvalibleForSpeech = false;
                break;
        }
       
        
    }

   void OnInit()
    {
        return;
    }
}
