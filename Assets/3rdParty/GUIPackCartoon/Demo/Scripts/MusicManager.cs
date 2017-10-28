// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // This class handles updating the music UI widgets depending on the player's selection.
    public class MusicManager : MonoBehaviour
    {
        private Slider m_musicSlider;
        private GameObject m_musicButton;

        private void Start()
        {
           
            m_musicSlider = GetComponent<Slider>();
            m_musicSlider.value = PlayerPrefs.GetInt("music_on");
            m_musicButton = GameObject.Find("MusicButton/Button");
        }

        public void SwitchMusic()
        {
            var backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>();
            backgroundMusic.GetComponent<AudioSource>().volume = m_musicSlider.value * backgroundMusic.Volume;
            PlayerPrefs.SetInt("music_on", (int)m_musicSlider.value);
            if (m_musicButton != null)
                m_musicButton.GetComponent<MusicButton>().ToggleSprite();
        }
    }
}
