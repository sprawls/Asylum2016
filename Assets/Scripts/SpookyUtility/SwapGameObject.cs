using UnityEngine;
using System.Collections;

public class SwapGameObject : MonoBehaviour {

    public GameObject startGO;
    public GameObject endGO;
    public GameObject GONotToLookAt;

    private IsRendered[] _GOIsRendered;

    private bool _playerInCollider = false;

    void Start() {
        _GOIsRendered = GONotToLookAt.GetComponentsInChildren<IsRendered>();
        StartCoroutine(CheckForSwap());
    }

    IEnumerator CheckForSwap() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            AttemptSwap();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _playerInCollider = true;
            AttemptSwap();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _playerInCollider = false;
        }
    }


    void AttemptSwap() {
        if (!IsVisible() && _playerInCollider) {
            if (startGO != null) startGO.SetActive(false);
            if (endGO != null) endGO.SetActive(true);
        }
    }

    bool IsVisible() {
        foreach (IsRendered isRend in _GOIsRendered) {
            if (isRend._visible) return true;
        }
        return false;
    }
}
