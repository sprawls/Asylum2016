using UnityEngine;
using System.Collections;

public class SwapGO : MonoBehaviour {

    public GameObject StartGO;
    public GameObject endGO;

    private bool _visible = false;
    private bool _playerInCollider = false;

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
    void OnBecameInvisible() {
        _visible = false;
        AttemptSwap();
    }
    void OnBecameVisible() {
        _visible = true;
    }

    void AttemptSwap() {
        if (!_visible && _playerInCollider) {
            StartGO.SetActive(false);
            endGO.SetActive(false);
        }
    }
}
