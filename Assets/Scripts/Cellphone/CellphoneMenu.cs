using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class CellphoneMenu : MonoBehaviour {
	[Header("Apps")]
	public Button phoneButton;
	public Button galleryButton;
	public Button flashlightButton;
	public Button quitButton;

	[Header("Icons")]
	public Sprite phoneIcon;
	public Sprite phoneRingingIcon;
	public Sprite flashlightOpenedIcon;
	public Sprite flashlightClosedIcon;

	public static event Action OnMenuOpen;
	public static event Action OnMenuClose;
	public static event Action OnPhoneOpen;
	public static event Action OnGalleryOpen;
	public static event Action OnFlashlightOpen;
	public static event Action OnFlashlightClose;
	public static event Action OnQuit;

	private bool _focused = false;
	private bool _flashlightOpened = true;
	private bool _flashlightWasOpened = false;
	private CanvasGroup _canvasGroup;

	private void Awake() {
		CameraController.OnCameraStart += this.OnCameraStart;
		CameraController.OnCameraEnd += this.OnCameraEnd;

		_canvasGroup = GetComponentInChildren<CanvasGroup>();
	}

	private void OnDestroy() {
		CameraController.OnCameraStart -= this.OnCameraStart;
		CameraController.OnCameraEnd -= this.OnCameraEnd;
	}

	private void Update() {
		if (Input.GetButtonDown("Flashlight")) {
			OnFlashlightIconClicked();
		}
        
		if (Input.GetButtonDown("Cellphone")) {
			if (!_focused) {
				OnFocus();
			} else {
				OnUnfocus();
			}
		}

		if (Input.GetButtonDown("Cancel") && _focused) {
			OnUnfocus();
		}
	}

	public void OnFocus() {
		EventSystem.current.SetSelectedGameObject(this.phoneButton.gameObject);
		_focused = true;

		if (OnMenuOpen != null) {
			OnMenuOpen.Invoke();
		}
	}

	public void OnUnfocus() {
		EventSystem.current.SetSelectedGameObject(null);
		_focused = false;

		if (OnMenuClose != null) {
			OnMenuClose.Invoke();
		}
	}

	public void OnPhoneIconClicked() {
		if (OnPhoneOpen != null) {
			OnPhoneOpen.Invoke();

			Image phoneIcon = phoneButton.transform.FindChild("Icon").GetComponent<Image>();
			if (phoneIcon != null) {
				phoneIcon.sprite = this.phoneIcon;
			}
		}

		// TODO: Play calls

		OnUnfocus();
	}

	public void OnGalleryIconClicked() {
		if (OnGalleryOpen != null) {
			OnGalleryOpen.Invoke();
		}

		// TODO: Open gallery app

		OnUnfocus();
	}

	public void OnFlashlightIconClicked() {
		_flashlightOpened = !_flashlightOpened;

		Image flashlightIcon = flashlightButton.transform.FindChild("Icon").GetComponent<Image>();
		if (flashlightIcon != null) {
			flashlightIcon.sprite = (_flashlightOpened) ? this.flashlightOpenedIcon : this.flashlightClosedIcon;
		}

		if (_flashlightOpened) {
			if (OnFlashlightOpen != null) {
				OnFlashlightOpen.Invoke();
			}
		} else {
			if (OnFlashlightClose != null) {
				OnFlashlightClose.Invoke();
			}
		}
	}

	public void OnQuitIconClicked() {
		if (OnQuit != null) {
			OnQuit.Invoke();
		}

		// TODO: Quit gaem

		OnUnfocus();
	}

	// TODO: Plug this to the receive call event
	public void OnReceiveCall() {
		Image phoneIcon = phoneButton.transform.FindChild("Icon").GetComponent<Image>();
		if (phoneIcon != null) {
			phoneIcon.sprite = this.phoneRingingIcon;
		}
	}

    private void OnCameraStart() {
        _canvasGroup.DOFade(0f, 0.25f);

		// Close flashlight when opening camera
		_flashlightWasOpened = _flashlightOpened;
		if (_flashlightOpened) {
			OnFlashlightIconClicked();
		}
    }

    private void OnCameraEnd() {
        _canvasGroup.DOFade(1f, 0.25f);

		// Reopen flashlight if it was opened
		if (_flashlightWasOpened) {
			OnFlashlightIconClicked();
		}
		_flashlightWasOpened = false;
    }
}
