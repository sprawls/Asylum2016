using UnityEngine;
using System.Collections;

public class IsRendered : MonoBehaviour {

    public bool _visible { get; private set; }

    void Awake() { _visible = true; }

    void OnBecameInvisible() {
        //Debug.Log("Invisible");
        _visible = false;
    }
    void OnBecameVisible() {
        //Debug.Log("Visible");
        _visible = true;
    }
}
