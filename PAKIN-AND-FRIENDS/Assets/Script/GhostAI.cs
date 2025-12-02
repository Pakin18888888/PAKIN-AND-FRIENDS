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

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (Player == null)
                Debug.LogError("GhostAI2D: NO PLAYER FOUND! Set Player tag correctly.");
        }
    }

    void Update()
    {
        if (Player == null)
            return;

        // Lock Z axis
        Vector3 ghostPos = transform.position;
        ghostPos.z = 0;
        transform.position = ghostPos;

        Vector3 playerPos = Player.position;
        playerPos.z = 0;

        float dist = Vector2.Distance(ghostPos, playerPos);

        // Debug
        Debug.Log("DIST = " + dist);

        if (dist <= detectRange)
        {
            Vector2 newPos = Vector2.MoveTowards(ghostPos, playerPos, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, ghostPos.z);
        }

        if (dist <= jumpScareDistance)
        {
            Debug.Log("Ghost TOUCH Player!");
            Destroy(gameObject);
        }

        if (dist >= disappearRange)
        {
            Debug.Log("Ghost disappear because distance = " + dist);
            Destroy(gameObject);
        }
    }
}
