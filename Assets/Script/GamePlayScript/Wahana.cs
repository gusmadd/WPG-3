using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wahana : MonoBehaviour
{
        private void OnTriggerStay(Collider other)
    {
        var p = other.GetComponent<PlayerControler>();
        if (p != null && Input.GetKeyDown(KeyCode.E))
        {
            p.hasTriedWahana = true;
        }
    }
}
