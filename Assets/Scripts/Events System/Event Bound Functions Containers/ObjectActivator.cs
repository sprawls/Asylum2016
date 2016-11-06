using UnityEngine;
using System.Collections.Generic;

public class ObjectActivator : MonoBehaviour, IEventBoundFunctions
{
    [EventBoundFunction]
    public void ActivateObject()
    {
        gameObject.SetActive(true);
    }

    [EventBoundFunction]
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}