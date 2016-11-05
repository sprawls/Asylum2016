using UnityEngine;
using System.Collections.Generic;

public class EventDispatcher_Activator : EventDispatcher
{
    [SerializeField]
    private List<EventListener_Activator> _events = new List<EventListener_Activator>();

    [EventBoundFunction]
    public void Activate()
    {
        for (int i = 0; i < _events.Count; ++i)
        {
            _events[i].Activate();
        }
    }
}