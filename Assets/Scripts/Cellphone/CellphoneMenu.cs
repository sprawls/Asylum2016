using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CellphoneMenu : MonoBehaviour {
	[Header("Apps")]
	public Button phoneButton;
	public Button cameraButton;
	public Button flashlightButton;
	public Button menuButton;

	[Header("Icons")]
	public Sprite phoneIcon;
	public Sprite phoneRingingIcon;
	public Sprite flashlightOpenedIcon;
	public Sprite flashlightClosedIcon;

	public static event Action OnPhoneOpen;
	public static event Action OnCameraOpen;
	public static event Action OnFlashlightOpen;
	public static event Action OnFlashlightClose;
	public static event Action OnMenuOpen;
	
	private bool flashlightOpened = false;

	public void OnPhoneIconClicked() {
		if (OnPhoneOpen != null) {
			OnPhoneOpen.Invoke();

			Image phoneIcon = phoneButton.transform.FindChild("Icon").GetComponent<Image>();
			if (phoneIcon != null) {
				phoneIcon.sprite = this.phoneIcon;
			}
		}

		EventSystem.current.SetSelectedGameObject(null);
	}

	public void OnCameraIconClicked() {
		if (OnCameraOpen != null) {
			OnCameraOpen.Invoke();
		}

		EventSystem.current.SetSelectedGameObject(null);
	}

	public void OnFlashlightIconClicked() {
		this.flashlightOpened = !this.flashlightOpened;

		Image flashlightIcon = flashlightButton.transform.FindChild("Icon").GetComponent<Image>();
		if (flashlightIcon != null) {
			flashlightIcon.sprite = (this.flashlightOpened) ? this.flashlightOpenedIcon : this.flashlightClosedIcon;
		}

		if (this.flashlightOpened) {
			if (OnFlashlightOpen != null) {
				OnFlashlightOpen.Invoke();
			}
		} else {
			if (OnFlashlightClose != null) {
				OnFlashlightClose.Invoke();
			}
		}

		EventSystem.current.SetSelectedGameObject(null);
	}

	public void OnMenuIconClicked() {
		if (OnMenuOpen != null) {
			OnMenuOpen.Invoke();
		}

		EventSystem.current.SetSelectedGameObject(null);
	}

	// TODO: Plug this to the receive call event
	public void OnReceiveCall() {
		Image phoneIcon = phoneButton.transform.FindChild("Icon").GetComponent<Image>();
		if (phoneIcon != null) {
			phoneIcon.sprite = this.phoneRingingIcon;
		}
	}
}
