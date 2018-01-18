using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {
 
    SceneTransition m_sceneTransition;

    PopupOpener m_popupOpener;

    void Awake()
    {
        GetReferences();
    }

    void GetReferences()
    {
        m_sceneTransition = GetComponent<SceneTransition>();
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void ButtonClicked()
    {
        if (!string.IsNullOrEmpty(LocalizationManager.Instance.firstLanguage) &&
            !string.IsNullOrEmpty(LocalizationManager.Instance.secondLanguage))
        {
            m_sceneTransition.PerformTransition();
                      
        }
        else
        {
            m_popupOpener.OpenPopup();
        }
    }
}
