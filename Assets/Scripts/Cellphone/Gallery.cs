using UnityEngine;
using System.Collections.Generic;

public class Gallery : MonoBehaviour {

	[SerializeField]
	private List<Sprite> _pictures;

	private bool _opened;

	private GameObject _gallery;

	private void Start () {
		CameraController.OnPictureTaken += this.OnPictureTaken;
		CellphoneMenu.OnGalleryOpen += this.Open;
		CellphoneMenu.OnGalleryClose += this.Close;

		_gallery = transform.FindChild("Canvas").FindChild("Gallery").gameObject;
	}
	
	private void OnPictureTaken(Sprite newPicture) {
		_pictures.Add(newPicture);
	}

	private void Open() {
		_gallery.SetActive(true);
	}

	private void Close() {
		_gallery.SetActive(false);
	}
}
