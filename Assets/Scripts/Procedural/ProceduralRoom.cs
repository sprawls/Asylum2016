using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralRoom : MonoBehaviour {

    [SerializeField] private RoomAnchor startAnchor;
    [SerializeField] private RoomAnchor endAnchor;
    [SerializeField] private GameObject RoomToSpawn;
    [SerializeField] private GameObject RoomToReplaceWith;

    [SerializeField] private float UnspawnDistance = 100f;
    private static GameObject player;


    void Start() {
        if (startAnchor == null) Debug.LogWarning("Start Anchor is null for " + gameObject.name);
        if (RoomToSpawn == null) Debug.LogWarning("No room to spawn for " + gameObject.name);
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(HandleUnspawnDistance());
    }

    public void PositionRoomToOtherAnchor(RoomAnchor otherAnchor) {
        //Debug.Log("Position : " + startAnchor.transform.position + "    rotation : " + startAnchor.transform.rotation);
        //Debug.Log("OtherPos : " + otherAnchor.transform.position + "    OtherRot : " + otherAnchor.transform.rotation);

        Vector3 requiredRotation = -startAnchor.transform.rotation.eulerAngles + otherAnchor.transform.rotation.eulerAngles;
        //Debug.Log("Rotation to do : " + requiredRotation);
        transform.Rotate(requiredRotation);

        Vector3 requiredTranslation = -startAnchor.transform.position + otherAnchor.transform.position;
        //Debug.Log("Translation to do : " + requiredTranslation);
        transform.Translate(requiredTranslation);
    }

    public void SpawnEndRoom() {
        GameObject go = (GameObject)Instantiate(RoomToSpawn, transform.position, Quaternion.identity);
        go.GetComponent<ProceduralRoom>().PositionRoomToOtherAnchor(endAnchor);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Replace()
    {
        RoomToReplaceWith.transform.position = gameObject.transform.position;
        Destroy();
    }

    IEnumerator HandleUnspawnDistance() {
        while (true) {
            if (player != null) {
                if ((transform.position - player.transform.position).magnitude > UnspawnDistance) Destroy();
                yield return new WaitForSeconds(1f);
            }
        }
    }


}

