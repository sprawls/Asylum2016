using UnityEngine;

public class CallUnlockEvent : MonoBehaviour, IEventBoundFunctions
{
    [SerializeField]
    private CallTrack _trackToUnlock;

    [EventBoundFunction]
    public void UnlockPhoneCall()
    {
        CallsController.UnlockCallAudioTrack(_trackToUnlock);
    }
}