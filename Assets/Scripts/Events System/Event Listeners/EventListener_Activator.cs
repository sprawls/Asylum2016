using UnityEngine;

[DisallowMultipleComponent]
public class EventListener_Activator : EventListener
{
    protected override string Description
    {
        get { return "Activated by other Event Callers."; }
    }

    public void Activate()
    {
        Trigger();
    }
}