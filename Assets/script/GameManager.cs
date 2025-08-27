using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Referensi")]
    public PlayerControler player;  //script player
    public CameraFollow camFollow; //script camera
    public GameObject wahana;
    public GameObject naratorPanel;
    public TextMeshProUGUI narratorText;
    private int step = 0;
    private bool tutorialStarted = false;

    void Start()
    {
        naratorPanel.SetActive(false);

        // Lock player dan kamera di awal
        player.canMove = false;
        player.isIntro = true;
        camFollow.isActive = false;

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(2f);  // delay sebelum mulai

        ShowNarator("Gunakan WASD untuk bergerak");
        player.canMove = true;       // aktifkan player
        tutorialStarted = true;      // mulai pantau di Update
    }

    void Update()
    {
        if (!tutorialStarted) return;  // ← TAMBAH: jangan proses step sebelum pesan WASD muncul
        // Step 0 -> player belajar jalan
        if (step == 0 && player.HasMoved())
        {
            step = 1;
            ShowNarator("Dekati wahana dan tekan E untuk mencoba");
        }

        // Step 1 -> player coba wahana
        if (step == 1 && player.hasTriedWahana)
        {
            step = 2;
            ShowNarator("Sekarang cari tiket! Tekan ENTER untuk lanjut");
        }

        // Step 2 -> player tekan ENTER
        if (step == 2 && Input.GetKeyDown(KeyCode.Return))
        {
            HideNarator();
            player.isIntro = false;   // player bebas
            camFollow.isActive = true; // kamera mulai follow
            step = 3; // selesai tutorial
        }
    }

    void ShowNarator(string message)
    {
        naratorPanel.SetActive(true);
        narratorText.text = message; // ← PASTIKAN pakai "narratorText"
    }

    void HideNarator()
    {
        naratorPanel.SetActive(false);
    }
}
