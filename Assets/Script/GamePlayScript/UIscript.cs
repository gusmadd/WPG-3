using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIscript : MonoBehaviour
{
    public static UIscript Instance; // biar gampang dipanggil dari mana saja
    public TextMeshProUGUI chaseNotif;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (chaseNotif != null)
            chaseNotif.gameObject.SetActive(false);
    }

    public void ShowChaseNotif(bool show)
    {
        if (chaseNotif != null)
            chaseNotif.gameObject.SetActive(show);
    }
}
