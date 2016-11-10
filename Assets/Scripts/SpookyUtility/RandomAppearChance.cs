using UnityEngine;
using System.Collections;

public class RandomAppearChance : MonoBehaviour {

    [Range(0f, 100f)]
    public float chance = 20f;

	// Use this for initialization
	void Start () {
        if (Random.Range(0f, 100f) >= chance) Destroy(gameObject);
	}
	
}
