// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine.SceneManagement;
using UnityEngine;

namespace Ricimi
{
    // This class represents the sound button that is used in several places in the demo.
    // It handles the logic to enable and disable the demo's sounds and store the player
    // selection to PlayerPrefs.
    public class SoundButton : MonoBehaviour
    {
        private SpriteSwapper m_spriteSwapper;
        private bool m_on;
        private BackgroundMusic m_bgMusic;

        private void Start()
        {
           

            m_bgMusic = GameObject.FindObjectOfType<BackgroundMusic>();
            m_spriteSwapper = GetComponent<SpriteSwapper>();
            m_on = PlayerPrefs.GetInt("sound_on") == 1;

            if (!m_on)
            {
                m_spriteSwapper.SwapSprite();
                m_bgMusic.GetComponent<AudioSource>().volume = 0f;
                
            }
            else
            {
                if(PlayerPrefs.GetInt("music_on") == 1)
                    m_bgMusic.GetComponent<AudioSource>().volume = m_bgMusic.Volume;
                else
                {
                    m_bgMusic.GetComponent<AudioSource>().volume = 0;
                }
            }

            //Toggle();
            //Toggle();
           

        }

        public void Toggle()
        {
            if (SceneManager.GetActiveScene().name == "Gameplay")
            {
                AudioListener.volume = 0;
                Debug.Log("gameplay scene");
            }
            else
            {

                if (m_on)
                {
                    m_bgMusic.FadeOut();
                }
                else
                {
                    m_bgMusic.FadeIn();
                }
                m_on = !m_on;
            
           
               AudioListener.volume = m_on ? 1 : 0;
               PlayerPrefs.SetInt("sound_on", m_on ? 1 : 0);
               PlayerPrefs.SetInt("music_on", m_on ? 1 : 0);
                
            }
          
               

        }

        public void ToggleSprite()
        {
            m_on = !m_on;
            m_spriteSwapper.SwapSprite();
        }
    }
}
