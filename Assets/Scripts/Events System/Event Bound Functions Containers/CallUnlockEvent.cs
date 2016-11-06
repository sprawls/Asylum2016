using UnityEngine;

public class CallUnlockEvent : MonoBehaviour, IEventBoundFunctions
{
    [SerializeField]
    private CallTrack _trackToUnlock;

    [EventBoundFunction]
    public void SendCallEvent()
    {
        CallsController.UnlockCallAudioTrack(_trackToUnlock);
    }
}