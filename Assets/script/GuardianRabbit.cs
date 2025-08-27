using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianRabbit : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(10, 0, 10);
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    [Header("Chase Settings")]
    public Transform player;
    public float detectionRadius = 6f;
    public float chaseSpeed = 4f;

    private Rigidbody rb;
    private Vector3 targetPoint;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        PickNewPatrolPoint();
    }

    void Update()
    {
        // hitung jarak player dengan kelinci
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // cek apakah player masuk radius
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            UIscript.Instance.ShowChaseNotif(true);  // tampil notif
        }
        else
        {
            isChasing = false;
            UIscript.Instance.ShowChaseNotif(false); // sembunyikan notif
        }
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            // kejar player
            Vector3 dir = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + dir * chaseSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector3 target = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);

        // bergerak ke target
        rb.MovePosition(Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime));

        // jika sudah dekat ke target → pilih target baru
        if (Vector3.Distance(transform.position, target) < 0.3f)
        {
            PickNewPatrolPoint();
        }
    }

    void PickNewPatrolPoint()
    {
        float randX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);
        targetPoint = areaCenter + new Vector3(randX, 0, randZ);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
