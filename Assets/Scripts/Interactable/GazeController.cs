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

    [UsedImplicitly]
    private void Awake()
    {
    }

    [UsedImplicitly]
    private void Update()
    {
        Interactable interactable = GetGazeSelection();

        if (_currentInteractable != interactable)
        {
            Debug.LogFormat("Changed interactable from {0} to {1}", _currentInteractable, interactable);
            if (OnInteractableHoverExit != null) OnInteractableHoverExit(_currentInteractable);
            _currentInteractable = interactable;
            if (OnInteractableHoverEnter != null) OnInteractableHoverEnter(_currentInteractable);
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