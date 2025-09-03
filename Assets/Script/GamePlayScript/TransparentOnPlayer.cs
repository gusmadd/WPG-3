using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentOnPlayer : MonoBehaviour
{

    private SpriteRenderer sr;
    private float normalAlpha = 1f;
    private float fadeAlpha = 0.4f; // transparan saat player di belakang

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter(Collider other) // PAKAI 3D
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = fadeAlpha;
            sr.color = c;
        }
    }

    void OnTriggerExit(Collider other) // PAKAI 3D
    {
        if (other.CompareTag("Player"))
        {
            Color c = sr.color;
            c.a = normalAlpha;
            sr.color = c;
        }
    }
}


