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
    private AnimationCurve _curve;

    [SerializeField]
    private Transform _pivot;

    private DoorState _state;
    private float _animCurrentTime;
    private float _animTime;

    [UsedImplicitly]
    protected override void Awake()
    {
        base.Awake();

        _animTime = _curve.keys[_curve.length - 1].time;

        if (_startsOpen)
        {
            _state = DoorState.Opened;
            _animCurrentTime = 0;
        }
        else
        {
            _state = DoorState.Closed;
            _animCurrentTime = _animTime;
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

    public void Open()
    {
        _state = DoorState.Opening;
    }

    public void Close()
    {
        _state = DoorState.Closing;
    }
}