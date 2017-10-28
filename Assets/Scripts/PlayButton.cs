using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

  

    private SceneTransition m_sceneTransition;
    private PopupOpener m_popupOpener;

    private void Awake()
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
