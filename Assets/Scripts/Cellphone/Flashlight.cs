using UnityEngine;

public class Flashlight : MonoBehaviour {

	public Light spotlight;

	public void Start() {
		CellphoneMenu.OnFlashlightOpen += this.OnFlashlightOpen;
		CellphoneMenu.OnFlashlightClose += this.OnFlashlightClose;
	}

	public void OnFlashlightOpen() {
		this.spotlight.enabled = true;
	}

	public void OnFlashlightClose() {
		this.spotlight.enabled = false;
	}
}
