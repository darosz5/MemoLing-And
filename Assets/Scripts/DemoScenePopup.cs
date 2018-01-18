using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class DemoScenePopup : MonoBehaviour {  
  
    public void Transition()
    {
        if (!GameControl.control.secondLaunch)
        {
            GameControl.control.ShowDemoPopup();
        }   
    }

    public void PlayDemo()
    {
        GameControl.control.IsPlayingDemo = true;
    }

    public void SkipDemo()
    {
        GameControl.control.IsPlayingDemo = false;
        GameControl.control.secondLaunch = true;
        GameControl.control.SavePlayerData();
    }
}
