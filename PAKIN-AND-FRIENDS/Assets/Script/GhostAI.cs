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
    if (Player == null) return;

    Vector2 ghostPos = rb.position;
    Vector2 playerPos = Player.position;

    float dist = Vector2.Distance(ghostPos, playerPos);

    if (dist <= detectRange)
    {
        Vector2 newPos = Vector2.MoveTowards(ghostPos, playerPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    if (dist <= jumpScareDistance)
    {
        Debug.Log("Ghost TOUCH Player!");
    }

    if (dist >= disappearRange)
    {
        Destroy(gameObject);
    }
}
    
}
