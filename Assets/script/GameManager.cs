using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public PlayerControler player;
    public CameraFollow camFollow;
    public DialogManager dialogManager; // 👉 drag di inspector

    [Header("Spots")]
    public Transform startSpot;
    public Transform counterSpot;

    void Start()
    {
        player.canMove = false;
        player.isIntro = true;
        if (camFollow) camFollow.isActive = false;

        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(0.5f);

        // 1. Narator
        bool done = false;
        dialogManager.ShowDialog("Narrator",
            "Welcome to Happy Amusement Park, dear visitor!\n" +
            "It seems like this is the first time you come here.\n" +
            "Since I'm in a good mood today, I'll tell you how to do things round here~",
            () => done = true);
        yield return new WaitUntil(() => done);

        // 2. Player
        done = false;
        dialogManager.ShowDialog("Player", "Wait..! Why am I suddenly here?", () => done = true);
        yield return new WaitUntil(() => done);

        // 3. Narator "Let's go..."
        done = false;
        dialogManager.ShowDialog("Narrator", "See the ticket counter on the right? Let's go there.", () =>
        {
            done = true;
        });
        yield return new WaitUntil(() => done);

        // 👉 Setelah ENTER terakhir, sembunyikan panel & jalankan auto move
        dialogManager.HideDialog();

        // Player jalan
        player.StartAutoMove(counterSpot.position, 2.8f);

        // tunggu sampai selesai jalan
        yield return new WaitUntil(() => player.IsAutoMoving == false);
        Debug.Log("Player sampai counter!");

        // 4. Staff
        done = false;
        dialogManager.ShowDialog("Counter staff",
            "You're first.. time.. here..?\nYou.. have to.. solve the puzzle.. to get the ticket...",
            () => done = true);
        yield return new WaitUntil(() => done);

        dialogManager.HideDialog();
        player.canMove = true;
        if (camFollow) camFollow.isActive = true;
    }

}
