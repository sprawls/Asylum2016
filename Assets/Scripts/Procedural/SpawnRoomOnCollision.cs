using UnityEngine;
using System.Collections;

public class SpawnRoomOnCollision : MonoBehaviour {

    [SerializeField] private ProceduralRoom currentRoom;
    private bool _activated = false;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if(!_activated) SpawnRoom();
        }
    }

    void SpawnRoom() {
        _activated = true;
        currentRoom.SpawnEndRoom();
    }
}
