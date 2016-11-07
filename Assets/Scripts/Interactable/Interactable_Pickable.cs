using System;

public class Interactable_Pickable : Interactable
{
    public event Action OnPickup;

    protected override void OnTrigger()
    {
        if (OnPickup != null) OnPickup();

        Destroy(gameObject, 0.1f); //Isn't actually picked up
    }
}