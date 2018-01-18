﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Ricimi;

public class WaitPopup : MonoBehaviour {

    [Header("References")]

    public Text dateText;

    DateTime m_oldDate;

    Popup m_popup; 

    void Awake()
    {
        m_oldDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("WheelDate")));
        m_popup = GetComponent<Popup>();
    }

    void Update()
    {
        TimeSpan difference = (m_oldDate.AddHours(12).Subtract(DateTime.Now));
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
