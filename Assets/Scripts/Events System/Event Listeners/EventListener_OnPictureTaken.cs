using UnityEngine;
using JetBrains.Annotations;

[RequireComponent(typeof(Renderer))]
public class EventListener_OnPictureTaken : EventListener
{
    private IsRendered _isRendered;

    protected override void Awake()
    {
        base.Awake();

        CameraController.OnPictureTaken += OnPictureTaken;

        _isRendered = gameObject.AddComponent<IsRendered>();
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
        CameraController.OnPictureTaken -= OnPictureTaken;
    }

    private void OnPictureTaken(Sprite sprite)
    {
        if (_isRendered._visible)
        {
            Trigger();
        }
    }
}