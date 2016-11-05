using UnityEngine;
using JetBrains.Annotations;

public class EventListener_OnStart : EventListener
{
    protected override string Description
    {
        get { return "Called on GameObject Start"; }
    }

    [UsedImplicitly]
    private void Start()
    {
        Trigger();
    }
}