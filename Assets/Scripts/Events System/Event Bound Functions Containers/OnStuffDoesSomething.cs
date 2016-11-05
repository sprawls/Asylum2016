using UnityEngine;

public class OnStuffDoesSomething : MonoBehaviour, IEventBoundFunctions
{
    [EventBoundFunction]
    public void DoStuff()
    {
        Debug.Log("Test");
    }
}