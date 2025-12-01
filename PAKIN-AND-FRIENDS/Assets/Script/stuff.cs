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
    public bool hasGhost = false;           // ตู้นี้ spawn ผี (วิ่งตาม)
    public bool isJumpScareCabinet = false; // ตู้นี้ jump-scare ทันที
    public bool ghostSpawned = false;       // ป้องกัน spawn ซ้ำ
    public GameObject ghostPrefab;          // prefab ของผี
    public float ghostSpawnDistance = 4f;   // ระยะห่างจากผู้เล่นที่ผีจะเกิด

    [Header("Jumpscare Settings (for jump-scare cabinets)")]
    public GameObject jumpScareUI;      // UI รูปผี (full-screen image)
    public AudioSource jumpScareSound;  // เสียง jumpscare
    public Camera mainCam;              // main camera (2D)
    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.3f;
    public float jumpScareDisplayTime = 1.0f; // เวลาที่รูปผีโชว์แล้วหาย

     float GetDistancePlayer()
    {
        // สมมติ Player มี tag ว่า "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return Mathf.Infinity;

        return Vector3.Distance(player.transform.position, transform.position);
    }

    void Start()
    {
        GJ.gameObject.SetActive(false);
        GJ1.gameObject.SetActive(false);
        
        pickUpAllowed = false;
    }

    void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
             if (isJumpScareCabinet && !ghostSpawned)
            {
                PlayJumpScare();
                ghostSpawned = true;
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
            GJ.gameObject.SetActive(isActive1);
            GJ1.SetActive(isActive);

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

        // spawn ฝั่งเดียวจากผู้เล่นไปด้านหน้า player โดยใช้ระยะ ghostSpawnDistance
        Vector3 dir = (transform.position - player.transform.position).normalized; // spawn ละแวกตู้ไปทางห่างผู้เล่น
        Vector3 spawnPos = player.transform.position + dir * ghostSpawnDistance;
        spawnPos.z = 0f;

        Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Spawned ghost from cabinet: " + gameObject.name);
    }
      private void PlayJumpScare()
    {
        StartCoroutine(JumpScareRoutine());
    }
     IEnumerator JumpScareRoutine()
    {
        if (jumpScareUI != null) jumpScareUI.SetActive(true);
        if (jumpScareSound != null) jumpScareSound.Play();

        // camera shake 2D
        if (mainCam != null)
        {
            Vector3 originalPos = mainCam.transform.localPosition;
            float t = 0f;
            while (t < shakeDuration)
            {
                mainCam.transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;
                t += Time.deltaTime; // ไม่ใช้ unscaled delta เพื่อให้เกมยังเล่นปกติ
                yield return null;
            }
            mainCam.transform.localPosition = originalPos;
        }

        // รอจนรูปหายเอง
        yield return new WaitForSeconds(jumpScareDisplayTime);

        if (jumpScareUI != null) jumpScareUI.SetActive(false);
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GJ.gameObject.SetActive(true);
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