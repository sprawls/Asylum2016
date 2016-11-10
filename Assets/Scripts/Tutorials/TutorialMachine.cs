using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialMachine : MonoBehaviour {
	public enum EMessageType {
		OpenCamera = 0,
		TakeImportantPictures,
		ClickToInteract
	}

	public static TutorialMachine Instance {
		get {
			return GameObject.Find("TutorialMachine").GetComponent<TutorialMachine>();
		}
	}

	[SerializeField] private float _animationDuration = 3.0f;

	[Header("Messages")]
	[SerializeField] [TextArea(3, 10)] private string _openCamera;
	[SerializeField] [TextArea(3, 10)] private string _takeImportantPictures;
	[SerializeField] [TextArea(3, 10)] private string _clickToInteract;

	private Text _textfield;
	public EMessageType MessageShown { get; private set; }
	
	protected void Start() {
		_textfield = transform.FindChild("Text").GetComponent<Text>();
		_textfield.DOFade(0.0f, 0.0f);
	}

	public void ShowMessage(EMessageType type) {
		string message = "";
		switch (type) {
			case EMessageType.OpenCamera:
				message = _openCamera;
				break;
			case EMessageType.TakeImportantPictures:
				message = _takeImportantPictures;
				break;
			case EMessageType.ClickToInteract:
				message = _clickToInteract;
				break;
		}

		_textfield.text = message;
		MessageShown = type;
		_textfield.DOFade(1.0f, _animationDuration);
	}

	public void HideMessage() {
		_textfield.DOFade(0.0f, _animationDuration);
	}

	public void HideMessage(EMessageType ifType) {
		if (MessageShown == ifType) {
			HideMessage();
		}
	}
}
