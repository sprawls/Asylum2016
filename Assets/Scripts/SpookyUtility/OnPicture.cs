using UnityEngine;
using System.Collections;

public class OnPicture : MonoBehaviour {

    public GameObject ObjectToBeVisible;
    public float maxRangeForPicture = 15f;
    public float maxAngleForPicture = 30f;
    public float maxAngleFacingForPicture = 30f;

    private IsRendered[] _IsRendered;
    private static GameObject _player;
    private bool _activated = false;
    private bool _pictureValid = false;
    private bool _unsuscribed = false;

	void Awake () {
        CameraController.OnBeforePictureTaken += OnBeforePictureTakenEvent;
        CameraController.OnPictureTaken += OnPictureTakenEvent;
        CameraController.OnAfterPictureTaken += OnAfterPictureTakenEvent;

        UpdateIsRenderedObjects();
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
	}

	void Destroy () {
        Unsubscribe();
	}

    void OnPictureTakenEvent(Sprite sprite) {
        if (!_activated && _pictureValid) {
            OnPictureTaken();
        }
    }

    void OnBeforePictureTakenEvent() {
        IsPictureValid();
        if (!_activated && _pictureValid) {
            OnBeforePictureTaken();
        }
    }

    void OnAfterPictureTakenEvent() {
        if (!_activated && _pictureValid) {
            OnAfterPictureTaken();
            _activated = true;
        }
    }

    void IsPictureValid() {
        if (IsRendered() && !IsObstructed() && IsInRange()) _pictureValid = true;
        else _pictureValid = false;
    }

    bool IsRendered() {
        foreach (IsRendered isRend in _IsRendered) {
            //Debug.Log(gameObject.name + "  " + isRend._visible);
            if (isRend._visible) return true;
        }
        return false;
    }

    bool IsObstructed() {      
        foreach (IsRendered isRend in _IsRendered) {
            Vector3 camToRenderer = isRend.transform.position - _player.transform.position;
            //Debug.Log("angle = " + Vector3.Angle(_player.transform.forward, camToRenderer));
            //Debug.Log("angle2 = " + Vector3.Angle(isRend.transform.forward, _player.transform.position - isRend.transform.position));

            if (Vector3.Angle(_player.transform.forward, camToRenderer) > maxAngleForPicture) {           
                return true;
            } else if (Vector3.Angle(isRend.transform.forward, _player.transform.position - isRend.transform.position) > maxAngleFacingForPicture) {
                return true;
            }
        }
        return false;
    }

    bool IsInRange() {
        //Debug.Log("distance = " + (ObjectToBeVisible.transform.position - _player.transform.position).magnitude);
        if ((ObjectToBeVisible.transform.position - _player.transform.position).magnitude < maxRangeForPicture) return true;
        else return false;
    }

    public virtual void OnBeforePictureTaken() {

    }

    public virtual void OnPictureTaken() {

    }

    public virtual void OnAfterPictureTaken() {

    }

    protected void UpdateIsRenderedObjects() {
        _IsRendered = ObjectToBeVisible.GetComponentsInChildren<IsRendered>();
    }

    protected void Unsubscribe() {
        if (!_unsuscribed) { 
            CameraController.OnBeforePictureTaken -= OnBeforePictureTakenEvent;
            CameraController.OnPictureTaken -= OnPictureTakenEvent;
            CameraController.OnAfterPictureTaken -= OnAfterPictureTakenEvent;
            _unsuscribed = true;
        }
    }
}
