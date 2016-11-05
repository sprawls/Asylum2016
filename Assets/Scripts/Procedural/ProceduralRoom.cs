using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralRoom : MonoBehaviour {

    [SerializeField] private RoomAnchor startAnchor;
    [SerializeField] private RoomAnchor endAnchor;
    [SerializeField] private GameObject RoomToSpawn;


    void Start() {
        if (startAnchor == null) Debug.LogWarning("Start Anchor is null for " + gameObject.name);
        if (RoomToSpawn == null) Debug.LogWarning("No room to spawn for " + gameObject.name);
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


}

