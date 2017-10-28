using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Ricimi;

public class WaitPopup : MonoBehaviour {

    public Text dateText;

    DateTime oldDate;

    private Popup m_popup; 

    private void Awake()
    {
        oldDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("WheelDate")));
        m_popup = GetComponent<Popup>();
    }

    private void Update()
    {
        TimeSpan difference = (oldDate.AddHours(12).Subtract(DateTime.Now));
        dateText.text = String.Format("{0:00}:{1:00}:{2:00}",
        difference.Hours,
        difference.Minutes,
        difference.Seconds);
        dateText.text += " sec.";
        if(difference <= TimeSpan.Zero)
        {
            m_popup.Close();
        }

    }
}
