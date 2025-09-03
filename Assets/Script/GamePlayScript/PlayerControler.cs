using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator anim;

    private float lastMoveX;
    private float lastMoveZ;
    public bool isIntro = true;
    public bool canMove = false;
    public bool hasTriedWahana = false;
    private bool isNearWahana = false;
    // 👇 Tambahin
    private bool isAutoMoving = false;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveZ", -1f);
        anim.SetBool("isMoving", false);

        lastMoveX = 0f;
        lastMoveZ = -1f;
    }

    void Update()
    {
        if (isNearWahana && Input.GetKeyDown(KeyCode.E))
        {
            hasTriedWahana = true;
            Debug.Log("Wahana dicoba!");
        }
    }

    void FixedUpdate()
    {
        if (isAutoMoving) return; // 👈 cegah input saat auto-move
        if (!canMove) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        bool isMoving = movement.magnitude > 0;
        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            lastMoveX = moveX;
            lastMoveZ = moveZ;
        }

        anim.SetFloat("MoveX", isMoving ? moveX : lastMoveX);
        anim.SetFloat("MoveZ", isMoving ? moveZ : lastMoveZ);
    }

    // === AUTO MOVE ===
    public bool IsAutoMoving => isAutoMoving;

    public void StartAutoMove(Vector3 targetPos, float speed = 3f)
    {
        StartCoroutine(AutoMoveRoutine(targetPos, speed));
    }

    private IEnumerator AutoMoveRoutine(Vector3 targetPos, float speed)
    {
        canMove = false;
        isAutoMoving = true;

        Vector3 targetFlat = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        while (Vector3.Distance(transform.position, targetFlat) > 0.1f) // toleransi lebih longgar
        {
            Vector3 dir = (targetFlat - transform.position).normalized;

            rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

            anim.SetBool("isMoving", true);
            anim.SetFloat("MoveX", dir.x);
            anim.SetFloat("MoveZ", dir.z);

            yield return new WaitForFixedUpdate();
        }

        transform.position = targetFlat; // snap ke target
        anim.SetBool("isMoving", false);

        isAutoMoving = false;
        canMove = true;
        Debug.Log("AutoMove selesai, sampai di counter");
    }
}
