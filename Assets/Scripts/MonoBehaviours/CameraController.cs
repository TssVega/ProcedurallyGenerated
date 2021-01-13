using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Transform playerTransform;
    private readonly float cameraZ = -10f;

    private void Awake() {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    private void LateUpdate() {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, cameraZ);
    }
}
