using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

    public Vector3 cellphoneCameraPosition;
    public Vector3 cellphoneCameraRotation;

    public bool cameraModeActive { get; private set; }

    private Camera _mainCamera;
    private DepthOfField _depthOfField;
    private CameraDistortionVision _cameraDistortionVision;
    private GameObject _cellphoneScreen;

    //Cellphone animation
    private bool _cameraModeRequested = false;
    private bool _inEquipCameraAnimation = false;

    //Cellphone Object
    private GameObject _cellphoneObject;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    //animation values
    private float _cellphoneEquipAnimTime = 0.3f;
    private float _inCameraModeFocalSize = 0.00f;
    private float _outCameraModeFocalSize = 2.00f;

	void Start () {
        _mainCamera = transform.Find("FirstPersonCharacter").GetComponent<Camera>();
        _depthOfField = GetComponentInChildren<DepthOfField>();
        _cameraDistortionVision = GetComponentInChildren<CameraDistortionVision>();
        _cellphoneScreen = _mainCamera.transform.FindChild("CameraScreen").gameObject;
        _cellphoneScreen.SetActive(false);

        GameObject tempCellPhone = _mainCamera.transform.FindChild("Cellphone").gameObject;
        SetCellphoneObject(tempCellPhone);
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            _cameraModeRequested = true;
        } else if (Input.GetMouseButtonUp(1)) {
            _cameraModeRequested = false;
        }

        HandlePhoneEquip();

	}

    void HandlePhoneEquip() {
        if (_inEquipCameraAnimation) {
            return;
        }
        else if (_cameraModeRequested && !cameraModeActive) {
            StartCoroutine(ActivateCameraMode());
        } else if (!_cameraModeRequested && cameraModeActive) {
            StartCoroutine(DeativateCameraMode());
        }
    }

    IEnumerator DeativateCameraMode() {
        _inEquipCameraAnimation = true;
        //_cameraDistortionVision.ChangeDistortion(false);
        cameraModeActive = false;
        _cellphoneScreen.SetActive(false);

        //DOTWEEN
        if (_cellphoneObject != null) {
            _cellphoneObject.transform.DOLocalMove(_originalPosition, _cellphoneEquipAnimTime);
            _cellphoneObject.transform.DOLocalRotateQuaternion(_originalRotation, _cellphoneEquipAnimTime);
        }
        //OTHER ANIM
        float startFocalSize = _depthOfField.focalSize;
        for (float i = 0f; i < 1f; i += Time.deltaTime / _cellphoneEquipAnimTime) {
            _depthOfField.focalSize = Mathf.Lerp(startFocalSize, _outCameraModeFocalSize, i);
            yield return null;
        }
        _depthOfField.focalSize = _outCameraModeFocalSize;

        //AFTER ANIMATION
        _inEquipCameraAnimation = false;
    }

    IEnumerator ActivateCameraMode() {
        _inEquipCameraAnimation = true;
        //_cameraDistortionVision.ChangeDistortion(true);
        cameraModeActive = true;

        // DOTWEEN
        if (_cellphoneObject != null) {
            _cellphoneObject.transform.DOLocalMove(cellphoneCameraPosition, _cellphoneEquipAnimTime);
            _cellphoneObject.transform.DOLocalRotate(cellphoneCameraRotation, _cellphoneEquipAnimTime);
        }
        //OTHER ANIM
        float startFocalSize = _depthOfField.focalSize;
        for (float i = 0f; i < 1f; i += Time.deltaTime / _cellphoneEquipAnimTime) {
            _depthOfField.focalSize = Mathf.Lerp(startFocalSize, _inCameraModeFocalSize, i);
            yield return null;
        }
        _depthOfField.focalSize = _inCameraModeFocalSize;

        //AFTER ANIMATION
        _cellphoneScreen.SetActive(true);
        _inEquipCameraAnimation = false;
    }

    public void SetCellphoneObject(GameObject cellphone) {
        _cellphoneObject = cellphone;
        _originalPosition = cellphone.transform.position;
        _originalRotation = cellphone.transform.rotation;
    }
}
