using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class stuff : MonoBehaviour
{
    [Header("UI Interact")]
    public TextMeshProUGUI GJ;
    public GameObject GJ1;
    public bool pickUpAllowed;

    [Header("Ghost Settings")]
    public bool hasGhost = false;       
    public bool isJumpScareCabinet = false; 
    public bool ghostSpawned = false;       
    public GameObject ghostPrefab;          
    public float ghostSpawnDistance = 4f;   

    // 🔥 ลบตัวแปร Jumpscare UI/Sound/Camera Shake ออกจากสคริปต์นี้

    void Start()
    {
        if (GJ != null) GJ.gameObject.SetActive(false);
        if (GJ1 != null) GJ1.gameObject.SetActive(false);
        
        pickUpAllowed = false;
    }

    void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            // ถ้าตู้นี้ jump-scare ทันที (ลบ PlayJumpScare() ออกไปแล้ว)
            if (isJumpScareCabinet && !ghostSpawned)
            {
                 // ถ้าใช้ isJumpScareCabinet ต้องเรียก Jumpscare จากระบบอื่น
                 // (แนะนำให้ใช้ SpawnGhost() แล้วให้ GhostAI2D Jumpscare แทน)
                 // หรือถ้าอยากให้ Jumpscare ทันที อาจต้อง Instantiate Ghost 
                 // และสั่งให้มันทำ Jumpscare โดยตรง
                 
                 // สำหรับตอนนี้ เราจะเน้นที่ hasGhost
                 
                 ghostSpawned = true; // ป้องกันการเรียกซ้ำ
                 return;
            }

            // ถ้าตู้นี้ spawn ผี
            if (hasGhost && !ghostSpawned)
            {
                SpawnGhost();
                ghostSpawned = true;
            }

            bool isActive = !GJ1.activeSelf;
            bool isActive1 = !GJ.gameObject.activeSelf;
            if (GJ != null) GJ.gameObject.SetActive(isActive1);
            if (GJ1 != null) GJ1.SetActive(isActive);

            if (isActive)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    
    private void SpawnGhost()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || ghostPrefab == null) return;

        // 🔥 Logic การสุ่มตำแหน่งรอบตู้
        Vector3 centerPoint = transform.position;
        
        // สุ่มมุม 0-360 องศา
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        // คำนวณทิศทางสุ่ม
        Vector3 randomDir = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        
        // คำนวณตำแหน่งเกิด
        Vector3 spawnPos = centerPoint + randomDir.normalized * ghostSpawnDistance;
        spawnPos.z = 0f;

        // 5. Instantiating
        GameObject newGhost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        
        // 6. กำหนด Player Transform ให้ GhostAI2D
        GhostAI2D ghostAI = newGhost.GetComponent<GhostAI2D>();
        if (ghostAI != null)
        {
            ghostAI.playerTransform = player.transform;
        }

        Debug.Log("Spawned ghost from cabinet: " + gameObject.name + " at " + spawnPos);
    }
    
    // 🔥 ลบฟังก์ชัน PlayJumpScare() และ JumpScareRoutine() ออกจากสคริปต์นี้

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (GJ != null) GJ.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(GJ != null)
            {
                GJ.gameObject.SetActive(false);
            }
            pickUpAllowed = false;
        }
    }
}