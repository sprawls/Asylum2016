using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

    public Vector3 cellphoneCameraPosition;
    public Vector3 cellphoneCameraRotation;

    public bool cameraModeActive { get; private set; }

    private Camera _mainCamera;
    private Camera _cellphoneCamera;
    private DepthOfField _depthOfField;
    private CameraDistortionVision _cameraDistortionVision;
    private GameObject _cellphoneScreen;

    //Cellphone animation
    private bool _cameraModeRequested = false;
    private bool _inEquipCameraAnimation = false;
    private bool _currentlyTakingPicture = false;

    //Cellphone Object
    private GameObject _cellphoneObject;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    //Picture
    private Light _pictureFlash;
    private Image _flashImage;

    //animation values
    private float _cellphoneEquipAnimTime = 0.3f;
    private float _inCameraModeFocalSize = 0.00f;
    private float _outCameraModeFocalSize = 2.00f;

	void Start () {
        _mainCamera = transform.Find("FirstPersonCharacter").GetComponent<Camera>();
        _cellphoneCamera = _mainCamera.transform.Find("CellphoneCamera").GetComponent<Camera>();
        _depthOfField = GetComponentInChildren<DepthOfField>();
        _cameraDistortionVision = GetComponentInChildren<CameraDistortionVision>();
        _cellphoneScreen = _mainCamera.transform.FindChild("CameraScreen").gameObject;
        _pictureFlash = _mainCamera.transform.FindChild("PictureFlash").GetComponent<Light>();
        _flashImage = transform.Find("Flash Canvas").GetComponent<Image>();
        _flashImage.transform.parent = null;

        StartCoroutine(DeativateCameraMode());

        GameObject tempCellPhone = _mainCamera.transform.FindChild("Cellphone").gameObject;
        SetCellphoneObject(tempCellPhone);
	}
	
	void Update () {
        //Temp Toggling
        if (Input.GetMouseButtonDown(1)) {
            _cameraModeRequested = true;
        } else if (Input.GetMouseButtonUp(1)) {
            _cameraModeRequested = false;
        }

        if (Input.GetMouseButtonDown(0) && !_inEquipCameraAnimation && cameraModeActive && !_currentlyTakingPicture) {
            StartCoroutine(TakePicture());
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
        _pictureFlash.gameObject.SetActive(false);

        //DOTWEEN
        if (_cellphoneObject != null) {
            _cellphoneObject.transform.DOLocalMove(_originalPosition, _cellphoneEquipAnimTime);
            _cellphoneObject.transform.DOLocalRotateQuaternion(_originalRotation, _cellphoneEquipAnimTime);
        }
        _flashImage.DOFade(0f, 0f);
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

    IEnumerator TakePicture() {
        _currentlyTakingPicture = true;

        _pictureFlash.gameObject.SetActive(true);
        SaveImage(_flashImage);

        _flashImage.DOFade(1f, 0.1f);
        yield return new WaitForSeconds(0.2f);
    
 
        //TODO send event here
        _pictureFlash.gameObject.SetActive(false);

        _flashImage.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);

        _currentlyTakingPicture = false;
    }

    void SaveImage(Image imgToChange) {
        Material imageMat = _cellphoneScreen.GetComponent<MeshRenderer>().material;
        //RenderTexture rt = _cellphoneCamera.targetTexture;
        //Texture2D texture = (imageMat.GetTexture as Texture2D);

        RenderTexture oldRT = _cellphoneCamera.targetTexture;
        RenderTexture rt = new RenderTexture(512, 512, 24);

        _cellphoneCamera.targetTexture = rt;
        _cellphoneCamera.Render();
        RenderTexture.active = rt;

        
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        texture.ReadPixels( new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        _cellphoneCamera.targetTexture = oldRT;

        imgToChange.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100);
    }
}
