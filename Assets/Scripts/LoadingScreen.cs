using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    Animator m_anim;

    void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        m_anim.Play("LoadscreenHide", -1, 0f);
    }
}
