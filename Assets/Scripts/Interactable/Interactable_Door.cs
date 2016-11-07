using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Interactable_Door : Interactable, IEventBoundFunctions
{
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

        _pivot.transform.rotation = Quaternion.Euler(new Vector3(0, _curve.Evaluate(_animCurrentTime), 0));
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

        _pivot.transform.rotation = Quaternion.Euler(new Vector3(0, _curve.Evaluate(_animCurrentTime), 0));
    }

    protected override void OnTrigger()
    {
        if (_locked)
            return;

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
        if (_locked)
            return;

        _state = DoorState.Opening;
    }

    [EventBoundFunction]
    public void Close()
    {
        if (_locked)
            return;

        _state = DoorState.Closing;
    }

    [EventBoundFunction]
    public void Lock()
    {
        _locked = true;
    }

    [EventBoundFunction]
    public void Unlock()
    {
        _locked = false;
    }
}