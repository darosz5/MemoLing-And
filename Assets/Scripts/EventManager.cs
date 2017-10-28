using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{

    private static EventManager instance = null;
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }

    public delegate void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);

    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void AddListener(EVENT_TYPE Event_Type, OnEvent Listener)
    {
        List<OnEvent> ListenList = null;

        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }

        ListenList = new List<OnEvent>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }

    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        List<OnEvent> ListenList = null;
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
            return;

        for (int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null))
            {
                ListenList[i](Event_Type, Sender, Param);
            }
        }
    }
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }
    //-----------------------------------------------------------
    //Remove all redundant entries from the Dictionary
    public void RemoveRedundancies()
    {
        //Create new dictionary
        Dictionary<EVENT_TYPE, List<OnEvent>> TmpListeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

        //Cycle through all dictionary entries
        foreach (KeyValuePair<EVENT_TYPE, List<OnEvent>> Item in Listeners)
        {
            //Cycle through all listeners
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                //If null, then remove item
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }
            //If items remain, then add to tmp dictionary
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        //Replace listeners with new dictionary
        Listeners = TmpListeners;
    }
    //-----------------------------------------------------------
    //Called on scene change. Clean up dictionary
    //void OnLevelWasLoaded()
    //{
    //    RemoveRedundancies();
    //}
    //-----------------------------------------------------------

}
