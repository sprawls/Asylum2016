using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlickeringLights : MonoBehaviour {

    public Light[] _lights;

    public Vector2 timeBetweenFlickersRange = new Vector2(2f,4f);
    public Vector2 lenghtOfFlickersRange = new Vector2(0.04f,0.2f);
    public int amountOfFlickersMin = 1;
    public int amountOfFlickersMax = 4;

	// Use this for initialization
	void Start () {
        _lights = transform.GetComponentsInChildren<Light>();
        StartCoroutine(ManageFlickers());
    }

    IEnumerator ManageFlickers() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(timeBetweenFlickersRange.x, timeBetweenFlickersRange.y));
            yield return(StartCoroutine(Flicker()));
        }
    }

    IEnumerator Flicker() {
        int rand = Random.Range((int)amountOfFlickersMin, (int)amountOfFlickersMax + 1);
        for (int i = 0; i < rand; ++i) {
            ToggleLights(false);
            yield return new WaitForSeconds(Random.Range(lenghtOfFlickersRange.x, lenghtOfFlickersRange.y));
            ToggleLights(true);
            yield return new WaitForSeconds(Random.Range(lenghtOfFlickersRange.x, lenghtOfFlickersRange.y));
        }
    }

    private void ToggleLights(bool active) {
        foreach (Light l in _lights) {
            l.enabled = active;
        }
    }
}
