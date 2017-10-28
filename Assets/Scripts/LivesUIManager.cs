using UnityEngine;
using ExaGames.Common;
using UnityEngine.UI;

public class LivesUIManager : MonoBehaviour {

 
	public LivesManager LivesManager;
   
    public Text LivesText;
  
    public Text TimeToNextLifeText;

    public void OnLivesChanged()
    {
        LivesText.text = LivesManager.LivesText;
    }

    /// <summary>
    /// Time to next life changed event handler, changes the label value.
    /// </summary>
    public void OnTimeToNextLifeChanged()
    {
        TimeToNextLifeText.text = LivesManager.RemainingTimeString;
    }
}
