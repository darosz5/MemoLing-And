using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsFiller : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    ParticleSystem m_effect;

    [SerializeField]
    Image m_star1;

    [SerializeField]
    Image m_star2;

    [SerializeField]
    Image m_star3;

    StarsValues m_starValues;

    int m_starsObtained;

    void Awake()
    {
        if (GameControl.control.IsPlayingDemo)
        {
            m_starValues = new StarsValues();
            m_starValues.Star1 = 100;
            m_starValues.Star1 = 300;
            m_starValues.Star1 = 500;
            return;
        }
           
        m_starValues = GameControl.control.levelStarsValues[LevelsManager.Instance.activeLevelNum - 1];
        m_starsObtained = 0;
    }

    public void FillStars(int score)
    {     
        if(m_starsObtained == 0)
        {
            m_star1.fillAmount = (float)score / (float)m_starValues.Star1;

            if (m_star1.fillAmount >= 1)
            {
                m_starsObtained++;
                PlayEffect(m_star1.transform.position);
            }               
        }
             
        if (m_starsObtained == 1)
        {
            m_star2.fillAmount = (float)(score - m_starValues.Star1) / 
                (float)(m_starValues.Star2 - m_starValues.Star1);

            if (m_star2.fillAmount >= 1)
            {
                m_starsObtained++;
                PlayEffect(m_star1.transform.position);
            }
        }
        if (m_starsObtained == 2)
        {
            m_star3.fillAmount = (float)(score - m_starValues.Star2) / 
                (float)(m_starValues.Star3 - m_starValues.Star2);

            if (m_star3.fillAmount >= 1)
            {
                m_starsObtained++;
                PlayEffect(m_star1.transform.position);
            }
        }        
    }

    void PlayEffect(Vector3 position)
    {
        //m_effect.transform.position = new Vector3(position.x, position.y, m_effect.transform.position.z);       
        m_effect.Play();
    }
}
