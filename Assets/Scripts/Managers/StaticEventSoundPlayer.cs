using UnityEngine;
using System.Collections;

public class StaticEventSoundPlayer : MonoBehaviour {

    [Header("AudioClips")]
    [SerializeField] private AudioClip _cameraStart;
    [SerializeField] private AudioClip _cameraStop;
    [SerializeField] private AudioClip _onPicture;

    void Awake() {
        CameraController.OnPictureTaken += PlayOnPictureTakenEventSound;
        //CameraController.OnCameraStart += OnCameraStart;
        //CameraController.OnCameraEnd += OnCameraEnd;
    }

    void PlayOnPictureTakenEventSound(Sprite sp) {
        SoundManager.Instance.PlaySingleSFX(_onPicture, ESFXType.ESFXType_PLAYER);
    }

    void PlayOnCameraStartSound() {
        SoundManager.Instance.PlaySingleSFX(_cameraStart, ESFXType.ESFXType_PLAYER);
    }

    void PlayOnCameraStopSound() {
        SoundManager.Instance.PlaySingleSFX(_cameraStop, ESFXType.ESFXType_PLAYER);
    }





}
