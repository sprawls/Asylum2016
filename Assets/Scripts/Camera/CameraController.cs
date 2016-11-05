using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

    public GameObject CellphonePrefab;

    public Vector3 cellphoneCameraPosition;
    public Vector3 cellphoneCameraRotation;

    public bool cameraModeActive { get; private set; }

    private Camera _mainCamera;
    private Camera _cellphoneCamera;
    private DepthOfField _depthOfField;
    private CameraDistortionVision _cameraDistortionVision;
    private GameObject _cellphoneScreen;

    //EVENT
    public static event Action<Sprite> OnPictureTaken;
    public static event Action OnPictureWillBeTaken;
    public static event Action OnCameraStart;
    public static event Action OnCameraEnd;

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
    private Image _flashWhiteImage;

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
        _flashWhiteImage = _flashImage.transform.Find("WhiteImg").GetComponent<Image>();
        _flashImage.transform.parent = null;

        StartCoroutine(DeativateCameraMode());

        GameObject tempCellPhone = _mainCamera.transform.FindChild("Cellphone").gameObject;
        if (CellphonePrefab != null) {
            GameObject CellGO = (GameObject) Instantiate(CellphonePrefab, transform.position, Quaternion.identity);
            CellGO.transform.parent = tempCellPhone.transform;
            CellGO.transform.localPosition = Vector3.zero;
            CellGO.transform.localRotation = Quaternion.Euler(0, 180, 0);
            CellGO.transform.localScale = Vector3.one;
        } 
        
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
        if (OnCameraEnd != null) {
            OnCameraEnd();
        }
        _inEquipCameraAnimation = true;
        cameraModeActive = false;
        _cellphoneScreen.SetActive(false);
        _pictureFlash.gameObject.SetActive(false);

        //DOTWEEN
        if (_cellphoneObject != null) {
            _cellphoneObject.transform.DOLocalMove(_originalPosition, _cellphoneEquipAnimTime);
            _cellphoneObject.transform.DOLocalRotateQuaternion(_originalRotation, _cellphoneEquipAnimTime);
        }
        _flashImage.DOFade(0f, 0f);
        _flashWhiteImage.DOFade(0f, 0f);
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
        if (OnCameraStart != null) {
            OnCameraStart();
        }
        _inEquipCameraAnimation = true;
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
        _originalPosition = cellphone.transform.localPosition;
        _originalRotation = cellphone.transform.localRotation;
    }

    IEnumerator TakePicture() {
        if(OnPictureWillBeTaken != null) {
            OnPictureWillBeTaken();
        }

        _currentlyTakingPicture = true;

        _pictureFlash.gameObject.SetActive(true);
        SaveImage(_flashImage);

        _flashImage.DOFade(1f, 0.1f);
        _flashWhiteImage.DOFade(0.5f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        _flashWhiteImage.DOFade(0f, 0.2f);
        yield return new WaitForSeconds(0.15f);
 
        //TODO send event here
        _pictureFlash.gameObject.SetActive(false);

        _flashImage.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);

        _currentlyTakingPicture = false;

        if (OnPictureTaken != null) { 
            OnPictureTaken(_flashImage.sprite);
        }
    }

    void SaveImage(Image imgToChange) {
        int pic_width = 1280;
        int pic_height = 800;

        Material imageMat = _cellphoneScreen.GetComponent<MeshRenderer>().material;
        //RenderTexture rt = _cellphoneCamera.targetTexture;
        //Texture2D texture = (imageMat.GetTexture as Texture2D);

        RenderTexture oldRT = _cellphoneCamera.targetTexture;
        RenderTexture rt = new RenderTexture(pic_width, pic_height, 24);

        _cellphoneCamera.targetTexture = rt;
        _cellphoneCamera.Render();
        RenderTexture.active = rt;


        Texture2D texture = new Texture2D(pic_width, pic_height, TextureFormat.RGB24, false);
        texture.ReadPixels( new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        _cellphoneCamera.targetTexture = oldRT;

        Sprite GeneratedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100); ;
        imgToChange.sprite = GeneratedSprite;
    }
}
