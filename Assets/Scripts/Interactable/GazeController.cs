#define DEBUG_RAYCAST

using System;
using UnityEngine;
using JetBrains.Annotations;

public class GazeController : MonoBehaviour
{
    [SerializeField]
    private float _distance = 10;

    [SerializeField]
    private LayerMask _blockingLayers;

    public static event Action<Interactable> OnInteractableHoverEnter;
    public static event Action<Interactable> OnInteractableHoverExit;

    private Interactable _currentInteractable = null;
    private bool _cameraOn = false;

    [UsedImplicitly]
    private void Awake()
    {
        CameraController.OnCameraStart += Callback_OnCameraOn;
        CameraController.OnCameraEnd += Callback_OnCameraOff;
    }

    private void Callback_OnCameraOn()
    {
        _cameraOn = true;
    }

    private void Callback_OnCameraOff()
    {
        _cameraOn = false;
    }

    [UsedImplicitly]
    private void Update()
    {
        Interactable interactable = _cameraOn ? null : GetGazeSelection();

        if (_currentInteractable != interactable)
        {
            Debug.LogFormat("Changed interactable from {0} to {1}", _currentInteractable, interactable);
            if (OnInteractableHoverExit != null) OnInteractableHoverExit(_currentInteractable);
            if (_currentInteractable != null) _currentInteractable.ClearTargeted();
            _currentInteractable = interactable;
            if (_currentInteractable != null) _currentInteractable.SetTargeted();
            if (OnInteractableHoverEnter != null) OnInteractableHoverEnter(_currentInteractable);
        }

        if (!_cameraOn && _currentInteractable != null && Input.GetButtonDown("Interact"))
        {
            _currentInteractable.Trigger();
        }
    }

    private Interactable GetGazeSelection()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, _distance, _blockingLayers);

#if DEBUG_RAYCAST
        Debug.DrawLine(transform.position, transform.position + transform.forward * _distance, Color.green, Time.deltaTime);
#endif

        if (hit.collider == null)
            return null;

        return hit.collider.gameObject.GetComponent<Interactable>();
    }
}