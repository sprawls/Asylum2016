using UnityEngine;
using System.Collections;

public class ShowCameraTutorialMessage : MonoBehaviour {

	[SerializeField] private float _waitTime = 10.0f;

	protected void Start () {
		StartCoroutine("ShowMessage");
	}
	
	private IEnumerator ShowMessage() {
		yield return new WaitForSeconds(_waitTime);
		TutorialMachine.Instance.ShowMessage(TutorialMachine.EMessageType.OpenCamera);
	}
}
