using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryConnection : MonoBehaviour {

    

    public void Retry()
    {
        GameControl.control.RestartConnection();
    }

    public void Stop()
    {
        GameControl.control.DontRestart();
    }
}
