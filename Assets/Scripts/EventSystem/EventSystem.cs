﻿using System;
using System.Collections.Generic;

public class EventSystem{
    
    #region Singleton
    private static EventSystem instanceInternal = null;
    public static EventSystem instance
    {
        get
        {
            if (instanceInternal == null)
            {
                instanceInternal = new EventSystem();
            }
            return instanceInternal;
        }
    }
    #endregion

    public delegate void EventDelegate<T>(T e) where T : BaseEvent;
    private delegate void EventDelegate(BaseEvent e);

    private Dictionary<Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

    public void Connect<T>(EventDelegate<T> del) where T : BaseEvent
    {
        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }
    }

    public void Disconnect<T>(EventDelegate<T> del) where T : BaseEvent
    {
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    public void Dispatch(BaseEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
        }
    }
}