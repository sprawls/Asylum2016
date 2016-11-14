using UnityEngine;
using System.Collections;

public class UnspawnRoomOnCollision : MonoBehaviour {

    [SerializeField] private ProceduralRoom currentRoom;
    private bool _activated = false;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if(!_activated) SpawnRoom();
        }
    }

    void SpawnRoom() {
        _activated = true;
        currentRoom.Replace();
    }
}
