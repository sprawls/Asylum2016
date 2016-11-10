using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class Interactable_Door : Interactable, IEventBoundFunctions
{
    [SerializeField]
    private AudioClip _sndDoorOpen;
    [SerializeField]
    private AudioClip _sndDoorClose;
    [SerializeField]
    private AudioClip _sndDoorUnlock;
    [SerializeField]
    private AudioClip _sndDoorLock;

    [SerializeField]
    private float     _delayBeforeUnlock;

    //private float 

    private enum DoorState
    {
        Closed,
        Opening,
        Opened,
        Closing,
    }

    [SerializeField]
    private bool _startsOpen;

    [SerializeField]
    private bool _locked = false;

    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private Transform _pivot;

    private DoorState _state;
    private float _animCurrentTime;
    private float _animTime;

    public override bool IsInteractable
    {
        get { return !_locked; }
    }

    [UsedImplicitly]
    private void Awake()
    {
        _animTime = _curve.keys[_curve.length - 1].time;

        if (_startsOpen)
        {
            _state = DoorState.Opened;
            _animCurrentTime = _animTime;
        }
        else
        {
            _state = DoorState.Closed;
            _animCurrentTime = 0;
        }

        _pivot.transform.localRotation = Quaternion.Euler(new Vector3(0, _curve.Evaluate(_animCurrentTime), 0));
    }

    [UsedImplicitly]
    private void Update()
    {
        switch (_state)
        {
            case DoorState.Closed:
            case DoorState.Opened:
                return;

            case DoorState.Opening:
                _animCurrentTime = Mathf.Min(_animTime, _animCurrentTime + Time.deltaTime);

                if (_animCurrentTime == _animTime) _state = DoorState.Opened;
                break;

            case DoorState.Closing:
                _animCurrentTime = Mathf.Max(0, _animCurrentTime - Time.deltaTime);

                if (_animCurrentTime == 0) _state = DoorState.Closed;
                break;
        }

        _pivot.transform.localRotation = Quaternion.Euler(new Vector3(0, _curve.Evaluate(_animCurrentTime), 0));
    }

    protected override void OnTrigger()
    {
        if(_locked)
        {
            SoundManager.Instance.PlaySingleSFX(_sndDoorLock, ESFXType.ESFXType_GAMEPLAY);
        }

        switch (_state)
        {
            case DoorState.Closed:
            case DoorState.Closing:
                Open();
                break;

            case DoorState.Opened:
            case DoorState.Opening:
                Close();
                break;
        }
    }

    [EventBoundFunction]
    public void Open()
    {
        if(_locked)
            return; 

        _state = DoorState.Opening;
        SoundManager.Instance.PlaySingleSFXWithRandomPitch(_sndDoorOpen, ESFXType.ESFXType_GAMEPLAY);
    }

    [EventBoundFunction]
    public void Close()
    {
        if (_locked)
            return;

        _state = DoorState.Closing;
        SoundManager.Instance.PlaySingleSFXWithRandomPitch(_sndDoorClose, ESFXType.ESFXType_GAMEPLAY);
    }

    [EventBoundFunction]
    public void Lock()
    {
        _locked = true;
    }

    [EventBoundFunction]
    public void Unlock()
    {
        StartCoroutine(WaitTimeUntilUnlock(_delayBeforeUnlock));
    }

    IEnumerator WaitTimeUntilUnlock(float delay)
    {
        yield return new WaitForSeconds(delay);
        _locked = false;
        SoundManager.Instance.PlaySingleSFX(_sndDoorUnlock, ESFXType.ESFXType_GAMEPLAY);
    }
}