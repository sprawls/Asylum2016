using UnityEngine;
using System.Collections;

public class OnPicture : MonoBehaviour {

    public GameObject ObjectToBeVisible;

    private IsRendered[] _IsRendered;

	void Awake () {
        CameraController.OnBeforePictureTaken += OnBeforePictureTakenEvent;
        CameraController.OnPictureTaken += OnPictureTakenEvent;
        CameraController.OnAfterPictureTaken += OnAfterPictureTakenEvent;

        _IsRendered = ObjectToBeVisible.GetComponentsInChildren<IsRendered>();
	}

	void Destroy () {
        CameraController.OnBeforePictureTaken -= OnBeforePictureTakenEvent;
        CameraController.OnPictureTaken -= OnPictureTakenEvent;
        CameraController.OnAfterPictureTaken -= OnAfterPictureTakenEvent;
	}

    void OnPictureTakenEvent(Sprite sprite) {
        if (IsVisible()) {
            OnPictureTaken();
        }
    }

    void OnBeforePictureTakenEvent() {
        if (IsVisible()) {
            OnBeforePictureTaken();
        }
    }

    void OnAfterPictureTakenEvent() {
        if (IsVisible()) {
            OnAfterPictureTaken();
        }
    }

    bool IsVisible() {
        foreach (IsRendered isRend in _IsRendered) {
            if (isRend._visible) return true;
        }
        return false;
    }

    public virtual void OnBeforePictureTaken() {

    }

    public virtual void OnPictureTaken() {

    }

    public virtual void OnAfterPictureTaken() {

    }
}
