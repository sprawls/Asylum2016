using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProceduralRoom : MonoBehaviour {

    public struct RoomToSpawn {
        public GameObject Prefab_Room;
        public RoomAnchor SpawnAnchor;

        public RoomToSpawn(GameObject pRoom, RoomAnchor sAnchor) {
            Prefab_Room = pRoom;
            SpawnAnchor = sAnchor;
        }
    }

    [SerializeField] private RoomAnchor startAnchor;
    [SerializeField] private RoomToSpawn RoomsToSpawn;


    void Start() {
        if (startAnchor == null) Debug.LogWarning("Start Anchor is null for " + gameObject.name);
        if (RoomsToSpawn.Prefab_Room == null) Debug.LogWarning("No room to spawn for " + gameObject.name);
    }

    public void PositionRoomToOtherAnchor(RoomAnchor otherAnchor) {
        Quaternion requiredRotation = Quaternion.FromToRotation(startAnchor.transform.rotation.eulerAngles, otherAnchor.transform.rotation.eulerAngles);
        Vector3 requiredTranslation = startAnchor.transform.position - otherAnchor.transform.position;

        transform.Rotate(requiredRotation.eulerAngles);
        transform.Translate(requiredTranslation);
    }

    public void SpawnEndRoom() {
        GameObject go = (GameObject) Instantiate(RoomsToSpawn.Prefab_Room, transform.position, Quaternion.identity);
        go.GetComponent<ProceduralRoom>().PositionRoomToOtherAnchor(startAnchor);
    }


}

