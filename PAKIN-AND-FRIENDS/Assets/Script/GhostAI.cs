using System.Collections;
using UnityEngine;

public class GhostAI2D : MonoBehaviour
{
    public Transform Player;
    public float detectRange = 20f;
    public float speed = 3f;
    public float disappearRange = 30f;
    public float jumpScareDistance = 0.8f;
    public GameObject jumpScareUI;
    public AudioSource jumpScareSound;
    public Camera mainCam;
    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.3f;
    public float jumpScareDisplayTime = 1f;
    private Vector3 lastPos;
    Rigidbody2D rb;
     bool isJumpScaring = false; // ป้องกันไม่ให้ทำซ้ำ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (Player == null)
                Debug.LogError("GhostAI2D: NO PLAYER FOUND! Set Player tag correctly.");
        }
    }
    void OnDrawGizmos()
{
    if (Player != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Player.position);
    }
}

    void FixedUpdate()
    {
        if (Player == null || isJumpScaring) return;

        Vector2 ghostPos = rb.position;
        Vector2 playerPos = Player.position;

        float dist = Vector2.Distance(ghostPos, playerPos);

        if (dist <= detectRange)
        {
            Vector2 newPos = Vector2.MoveTowards(ghostPos, playerPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        // 🔥 เข้าใกล้แล้ว → ทำ Jump Attack
        if (dist <= jumpScareDistance)
        {
            StartCoroutine(JumpAttack());
        }

        if (dist >= disappearRange)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator JumpAttack()
    {
        isJumpScaring = true;

        // 1) พุ่งเข้าหาผู้เล่น + กระโดดขึ้นนิดนึง
        float jumpHeight = 1.5f;
        float jumpTime = 0.2f;

        Vector3 startPos = transform.position;
        Vector3 endPos = Player.position;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / jumpTime;

            // Parabolic jump
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, endPos, t) + new Vector3(0, height, 0);

            yield return null;
        }

        // 2) เรียก jumpscare จริงๆ
        if (JumpScare.Instance != null)
            JumpScare.Instance.PlayJumpScare();

        // 3) รอจนจบ jumpscare UI
        yield return new WaitForSeconds( JumpScare.Instance.jumpScareDisplayTime );

        // 4) ผีหายตัว
        Destroy(gameObject);
    }
}
    

