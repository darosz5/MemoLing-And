using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ricimi;

public class DayPassedCheck : MonoBehaviour {

    PopupOpener m_popupOpener;

    public GameObject SpinWheelPopup;
    public GameObject WaitPopup;

    DateTime currentDate;
    DateTime oldDate;

    private void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void CheckIfDayPassed()
    {
        
        if(PlayerPrefs.HasKey("WheelDate"))
        {
            currentDate = DateTime.Now;
            long old = Convert.ToInt64(PlayerPrefs.GetString("WheelDate"));
            oldDate = DateTime.FromBinary(old);

            TimeSpan difference = currentDate.Subtract(oldDate);

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
