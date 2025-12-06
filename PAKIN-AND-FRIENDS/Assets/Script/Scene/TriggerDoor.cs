using UnityEngine;
using System.Linq; // จำเป็นสำหรับ .Contains

public class TriggerDoor : MonoBehaviour
{
    public string doorID; 
    
    [Header("สถานะ")]
    public bool isLocked = true;

    private Collider2D doorCollider; 
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // --- 1. ตรวจสอบกับ GameManager ว่าประตูนี้เคยเปิดไปหรือยัง ---
        if (GameManager.Instance != null)
        {
            // ถ้าในความทรงจำมีชื่อประตูนี้อยู่ แสดงว่าเคยเปิดแล้ว
            if (GameManager.Instance.sceneState.openedDoors.Contains(doorID))
            {
                OpenInstant(); // สั่งให้เปิดทันทีโดยไม่ต้องไข
            }
        }
    }

    public void UnlockAndOpen()
    {
        if (!isLocked) return; 

        // เช็คกุญแจจาก Inventory
        if (CheckIfPlayerHasKey())
        {
            Debug.Log("ไขกุญแจสำเร็จ: " + doorID);
            isLocked = false;
            OpenDoor();
        }
        else
        {
            Debug.Log("ไม่มีกุญแจสำหรับประตู: " + doorID);
        }
    }

    bool CheckIfPlayerHasKey()
    {
        if (Inventory.Instance == null) return false;
        return Inventory.Instance.keyIDs.Contains(doorID);
    }

    public void OpenDoor()
    {
        // --- 2. บันทึกชื่อประตูลง GameManager ---
        if(GameManager.Instance != null)
        {
            // ถ้ายังไม่มีชื่อในลิสต์ ให้เพิ่มเข้าไป
            if (!GameManager.Instance.sceneState.openedDoors.Contains(doorID))
            {
                GameManager.Instance.sceneState.openedDoors.Add(doorID);
            }
        }

        OpenAnimation();
    }

    void OpenInstant() 
    {
        isLocked = false;
        if(doorCollider) doorCollider.enabled = false;
        if(spriteRenderer) spriteRenderer.enabled = false; 
    }

    void OpenAnimation() 
    {
        // ใส่ Effect เปิดประตู หรือ Animation ตรงนี้
        if(doorCollider) doorCollider.enabled = false;
        if(spriteRenderer) spriteRenderer.enabled = false; 
    }
}