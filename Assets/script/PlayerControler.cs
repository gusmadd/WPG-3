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
    public Camera mainCamera;          // ← TAMBAH (drag Camera di Inspector; kalau lupa akan auto Camera.main)
    private bool everMoved = false;    // ← TAMBAH: untuk HasMoved()

    private bool isNearWahana = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main; // ← TAMBAH

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // pastikan rigidbody tidak rotasi
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // atur collision detection lebih teliti
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // set default animasi ke bawah
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
            hasTriedWahana = true;   // kasih tahu GameManager
            Debug.Log("Wahana dicoba!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Masuk trigger dengan: " + other.name);

    if (other.CompareTag("Wahana"))
    {
        isNearWahana = true;
        Debug.Log("Dekat Wahana");
    }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Keluar trigger dengan: " + other.name);

    if (other.CompareTag("Wahana"))
    {
        isNearWahana = false;
        Debug.Log("Menjauh dari Wahana");
    }
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // ini cara yang aman biar tidak tembus collider
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // tandai pernah bergerak
        if (movement.sqrMagnitude > 0.0001f) everMoved = true;
        // animasi
        bool isMoving = movement.magnitude > 0;
        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            lastMoveX = moveX;
            lastMoveZ = moveZ;
        }

        anim.SetFloat("MoveX", isMoving ? moveX : lastMoveX);
        anim.SetFloat("MoveZ", isMoving ? moveZ : lastMoveZ);
        // kunci dalam kamera selama intro
        if (isIntro) ClampToCameraBounds();

    }
    public bool HasMoved()
    {
        return everMoved; // ← GANTI
    }
    void ClampToCameraBounds()
    {
        if (mainCamera == null) return;

        // Proyeksi sudut kamera ke bidang horizontal di ketinggian player
        Plane plane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

        Vector3[] corners = new Vector3[4];
        Ray r0 = mainCamera.ViewportPointToRay(new Vector3(0, 0, 0)); // kiri-bawah
        Ray r1 = mainCamera.ViewportPointToRay(new Vector3(1, 0, 0)); // kanan-bawah
        Ray r2 = mainCamera.ViewportPointToRay(new Vector3(0, 1, 0)); // kiri-atas
        Ray r3 = mainCamera.ViewportPointToRay(new Vector3(1, 1, 0)); // kanan-atas

        float d;
        if (plane.Raycast(r0, out d)) corners[0] = r0.GetPoint(d);
        if (plane.Raycast(r1, out d)) corners[1] = r1.GetPoint(d);
        if (plane.Raycast(r2, out d)) corners[2] = r2.GetPoint(d);
        if (plane.Raycast(r3, out d)) corners[3] = r3.GetPoint(d);

        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minZ = Mathf.Min(corners[0].z, corners[1].z, corners[2].z, corners[3].z);
        float maxZ = Mathf.Max(corners[0].z, corners[1].z, corners[2].z, corners[3].z);

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.z = Mathf.Clamp(p.z, minZ, maxZ);
        transform.position = p;
    }
}
