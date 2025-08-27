using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.5f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 offset;

    [Header("Tutorial Mode")]
    public bool isActive = true;  // default: kamera diam saat tutorial

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (!isActive) return; // kalau tidak aktif, kamera diam

        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
    }
}
