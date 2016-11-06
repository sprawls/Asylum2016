using UnityEngine;
using System.Collections;

public class RoomAnchor : MonoBehaviour {

     //[SerializeField] private RoomAnchor Prefab_Room;

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
