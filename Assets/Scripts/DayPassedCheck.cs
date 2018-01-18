using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ricimi;

public class DayPassedCheck : MonoBehaviour {

    [Header("References")]

    public GameObject SpinWheelPopup;

    public GameObject WaitPopup;

    PopupOpener m_popupOpener;

    DateTime m_currentDate;

    DateTime m_oldDate;

    void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void CheckIfDayPassed()
    {       
        if(PlayerPrefs.HasKey("WheelDate"))
        {
            m_currentDate = DateTime.Now;
            long old = Convert.ToInt64(PlayerPrefs.GetString("WheelDate"));
            m_oldDate = DateTime.FromBinary(old);

            TimeSpan difference = m_currentDate.Subtract(m_oldDate);

            if (difference.TotalHours >= 12)
            {
                m_popupOpener.popupPrefab = SpinWheelPopup;
                m_popupOpener.OpenPopup();
            }
            else
            {
                m_popupOpener.popupPrefab = WaitPopup;
                m_popupOpener.OpenPopup();
            }
        }
        else
        {          
            m_popupOpener.popupPrefab = SpinWheelPopup;
            m_popupOpener.OpenPopup();
        }
        

    }
}
