using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.5f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 offset;

    void Start()
    {
        // Simpan jarak awal kamera dengan player
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Posisi target = player + offset
        Vector3 targetPosition = player.position + offset;

        // Smooth follow
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
    }
}
