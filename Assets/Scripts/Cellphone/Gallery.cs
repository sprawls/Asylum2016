using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gallery : MonoBehaviour {
	[SerializeField]
	private List<Sprite> _pictures;

	[SerializeField]
	private Image _galleryPicturePrefab;

	private GameObject _gallery;
	private ScrollRect _scrollRect;
	private RectTransform _content;

	protected void Start() {
		CameraController.OnPictureTaken += this.OnPictureTaken;
		CellphoneMenu.OnGalleryOpen += this.Open;
		CellphoneMenu.OnGalleryClose += this.Close;

		_gallery = transform.FindChild("Canvas").FindChild("Gallery").gameObject;
		_scrollRect = _gallery.transform.FindChild("Scroll View").GetComponent<ScrollRect>();
		_content = _scrollRect.transform.FindChild("Viewport").FindChild("Content").GetComponent<RectTransform>();
	}

	protected void Update() {
		if (_gallery.activeSelf) {
			float verticalPos = _scrollRect.verticalNormalizedPosition;
			verticalPos += _scrollRect.scrollSensitivity * Input.mouseScrollDelta.y;
			_scrollRect.verticalNormalizedPosition = Mathf.Clamp(verticalPos, 0f, 1f);
		}
	}

	private void OnPictureTaken(Sprite newPicture) {
		_pictures.Add(newPicture);
		Image newElement = (Image) Instantiate(_galleryPicturePrefab, _content);
		newElement.sprite = newPicture;

		newElement.transform.localPosition = Vector3.zero;
		newElement.transform.localRotation = Quaternion.identity;
		newElement.transform.localScale = Vector3.one;
	}

	private void Open() {
		_gallery.SetActive(true);
	}

	private void Close() {
		_gallery.SetActive(false);
	}
}
