using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BE2_MainEventsManager : MonoBehaviour
{
    static BE2_EventsManager _instance;
    public static BE2_EventsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BE2_EventsManager();
            return _instance;
        }
    }

    void Init()
    {
        if (_instance == null)
            _instance = new BE2_EventsManager();
    }

    void Awake()
    {
        Init();
    }
}

/*
public class BE2_EventsManager : MonoBehaviour
{
    private Dictionary<BE2EventTypes, UnityEvent> eventDictionary;

    static BE2_EventsManager instance;
    public static BE2_EventsManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(BE2_EventsManager)) as BE2_EventsManager;

                if (instance)
                {
                    instance.Init();
                }
            }

            return instance;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<BE2EventTypes, UnityEvent>();
        }
    }

    void Awake()
    {
        instance = this;
        Init();
    }

    public static void StartListening(BE2EventTypes eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(BE2EventTypes eventName, UnityAction listener)
    {
        if (Instance == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(BE2EventTypes eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}

*/
