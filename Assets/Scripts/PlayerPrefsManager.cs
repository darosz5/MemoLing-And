using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string VOLUME_ON = "volume_on";
    const string DIFFICULTY_KEY = "difficulty";  

    public static void SwitchVolume(float volume)
    {
        PlayerPrefs.SetFloat(VOLUME_ON, volume);
    }

    public static float GetVolumeSwitch()
    {
       return PlayerPrefs.GetFloat(VOLUME_ON);
    }
    
}
