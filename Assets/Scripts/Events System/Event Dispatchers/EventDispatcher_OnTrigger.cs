using JetBrains.Annotations;
using UnityEngine;

public class EventDispatcher_OnTrigger : EventDispatcher
{
    [SerializeField]
    private EventListener_OnTriggerEnter[] _enterEvents;

    [SerializeField]
    private EventListener_OnTriggerExit[] _exitEvents;

    [UsedImplicitly]
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player")
            return;

        for (int i = 0; i < _enterEvents.Length; ++i)
        {
            _enterEvents[i].OnEnter();
        }
    }

    [UsedImplicitly]
    private void OnTriggerExit(Collider col)
    {
        if (col.tag != "Player")
            return;

        for (int i = 0; i < _exitEvents.Length; ++i)
        {
            _exitEvents[i].OnExit();
        }
    }
}