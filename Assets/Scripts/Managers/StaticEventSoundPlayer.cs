using UnityEngine;
using System.Collections;

public class StaticEventSoundPlayer : MonoBehaviour {

    [Header("AudioClips - Camera")]
    [SerializeField] private AudioClip _cameraStart;
    [SerializeField] private AudioClip _cameraStop;
    [SerializeField] private AudioClip _onPicture;
    [SerializeField] private AudioClip _onImportantPicture;


    [Header("AudioClips - FlashLight")]
    [SerializeField] private AudioClip _flashlightOpen;
    [SerializeField] private AudioClip _flashlightClose;
    [SerializeField] private AudioClip _flashLightBlocked;

    void Awake() {
        CameraController.OnPictureTaken += PlayOnPictureTakenEventSound;
        CameraController.OnCameraStart += PlayOnCameraStartSound;
        CameraController.OnCameraEnd += PlayOnCameraStopSound;
        CameraSight.OnImportantPictureTaken += PlayOnImportantPictureSound;

        CellphoneMenu.OnFlashlightOpen += PlayOnFlashlightOpenSound;
        CellphoneMenu.OnFlashlightClose += PlayOnFlashlightClosedSound;
        CellphoneMenu.OnOpenBlockedFlashlight += PlayOnFlashlightBlockedSound;
    }

    ///////////////////////////////// CAMERA /////////////////////////////////////
    void PlayOnPictureTakenEventSound(Sprite sp) {
        SoundManager.Instance.PlaySingleSFX(_onPicture, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnPictureTakenEventSound");
    }
    void PlayOnCameraStartSound() {
        SoundManager.Instance.PlaySingleSFX(_cameraStart, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnCameraStartSound");
    }
    void PlayOnCameraStopSound() {
        SoundManager.Instance.PlaySingleSFX(_cameraStop, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnCameraStopSound");
    }
    void PlayOnImportantPictureSound(Sprite sp) {
        SoundManager.Instance.PlaySingleSFX(_onImportantPicture, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnImportantPictureSound");
    }


    ///////////////////////////////// CAMERA /////////////////////////////////////
    void PlayOnFlashlightOpenSound() {
        SoundManager.Instance.PlaySingleSFX(_flashlightOpen, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnFlashlightOpenSound");
    }
    void PlayOnFlashlightClosedSound() {
        SoundManager.Instance.PlaySingleSFX(_flashlightClose, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnFlashlightClosedSound");
    }
    void PlayOnFlashlightBlockedSound() {
        SoundManager.Instance.PlaySingleSFX(_flashLightBlocked, ESFXType.ESFXType_PLAYER);
        //Debug.Log("PlayOnFlashlightBlockedSound");
    }





}
