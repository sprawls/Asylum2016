using UnityEngine;
using System.Collections.Generic;

public class DoorController : MonoBehaviour, IEventBoundFunctions
{
    [EventBoundFunction]
    public void Open()
    {
        Debug.Log("Open Door");
    }
}