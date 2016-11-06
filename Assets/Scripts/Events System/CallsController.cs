using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public enum CallTrack
{
    None,
}

public class CallsController : MonoBehaviour
{
    [Serializable]
    private class CallInfo
    {
        public CallTrack track;
        public AudioClip clip;
    }

    [SerializeField]
    private CallInfo[] _rawCalls;

    private static CallsController _instance;

    [SerializeField]
    private AudioSource _callSource;
    private Dictionary<CallTrack, AudioClip> _audioClips = new Dictionary<CallTrack, AudioClip>();
    private CallTrack _playingClip;

    public static event Action<CallTrack> OnCallUnlocked;
    public static event Action<CallTrack> OnCallStarted;
    public static event Action<CallTrack> OnCallEnded;

    [UsedImplicitly]
    void Awake()
    {
        _instance = this;

        _callSource.loop = false;

        for (int i = 0; i < _rawCalls.Length; ++i)
        {
            CallInfo info = _rawCalls[i];

            if (_audioClips.ContainsKey(info.track))
                continue;;

            _audioClips.Add(info.track, info.clip);
        }
    }

    [UsedImplicitly]
    private void Update()
    {
        if (_playingClip != CallTrack.None)
        {
            if (!_callSource.isPlaying)
            {
                if (OnCallEnded != null) OnCallEnded(_playingClip);
                _playingClip = CallTrack.None;
            }
        }
    }

    public void PlayAudioTrack(CallTrack track)
    {
        if (_audioClips.ContainsKey(track))
        {
            _playingClip = track;
            _callSource.clip = _audioClips[track];
            _callSource.Play();

            if (OnCallStarted != null) OnCallStarted(track);
        }
    }

    public static void UnlockCallAudioTrack(CallTrack call)
    {
        if (call == CallTrack.None)
            return;

        if (OnCallUnlocked != null) OnCallUnlocked(call);
    }

    public static void PlayCallAudioTrack(CallTrack call)
    {
        if (call == CallTrack.None)
            return;

        _instance.PlayAudioTrack(call);   
    }
}