using UnityEngine;

[RequireComponent(typeof(Interactable_Pickable))]
public class EventListener_OnPickup : EventListener
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<Interactable_Pickable>().OnPickup += Callback_OnPickup;
    }

    private void Callback_OnPickup()
    {
        Trigger();
    }
}