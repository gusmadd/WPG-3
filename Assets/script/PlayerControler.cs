using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //set default ke bawah
        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveZ", -1f);
        anim.SetBool("isMoving", false);

        lastMoveX = 0f;
        lastMoveZ = -1f;
    }
    private float lastMoveX;
    private float lastMoveZ;
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        // Movement
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // cek apakah bergerak
        bool isMoving = (moveX != 0 || moveZ != 0);
        anim.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            lastMoveX = moveX;
            lastMoveZ = moveZ;
        }

        // Kirim ke animator
        anim.SetFloat("MoveX", isMoving ? moveX : lastMoveX);
        anim.SetFloat("MoveZ", isMoving ? moveZ : lastMoveZ);
    }
}
