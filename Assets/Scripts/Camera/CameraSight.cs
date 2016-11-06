using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Renderer))]
public class CameraSight : MonoBehaviour {

    public static event Action<Sprite> OnImportantPictureTaken;
    public event Action OnImportantPictureTakenNonStatic;

    private Camera _cellphoneCamera;
    private Renderer _renderer;
    private bool _isSeen;

    [SerializeField]
    private float _maximumDistanceFromSightOnImportantPicture = 20.0f;

    [Range(0.0f,90.0f)]
    [SerializeField]
    private float _maximumLookAtAngle = 45.0f;

    [Range(0.0f, 90.0f)]
    [SerializeField]
    private float _maximumAngleFacing = 45.0f;

    void Awake()
    {
        GameObject cellphoneCamera = GameObject.FindGameObjectWithTag("CellphoneCamera");
        _cellphoneCamera = cellphoneCamera.GetComponentInChildren<Camera>();
        _renderer = this.GetComponent<Renderer>();

        CameraController.OnPictureTaken += this.OnPictureTaken;
    }

    void OnDestroy()
    {
        CameraController.OnPictureTaken -= this.OnPictureTaken;
    }

    void OnWillRenderObject()
    {
        Vector3 camToRenderer = _renderer.transform.position - _cellphoneCamera.transform.position;
        float distance = camToRenderer.magnitude;
        if (distance > _maximumDistanceFromSightOnImportantPicture)
        {
            _isSeen = false;
            return;
        }
        else if(Vector3.Angle(_cellphoneCamera.transform.forward, camToRenderer)  > _maximumLookAtAngle)
        {
            _isSeen = false;
            return;
        }
        else if(Vector3.Angle(_renderer.transform.forward, _cellphoneCamera.transform.position - _renderer.transform.position) > _maximumAngleFacing)
        {
            _isSeen = false;
            return;
        }
        else
        {
            Ray ray = new Ray(_cellphoneCamera.transform.position, camToRenderer);
            _isSeen = !Physics.Raycast(ray, distance, Physics.DefaultRaycastLayers , QueryTriggerInteraction.Ignore);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }
    }

    void OnBecameInvisible()
    {
        _isSeen = false;
    }

    void OnPictureTaken(Sprite picture)
    {
        if(_isSeen)
        {
            OnImportantPictureTaken(picture);
            OnImportantPictureTakenNonStatic();
        }
    }

    

}
