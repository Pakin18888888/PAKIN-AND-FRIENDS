using UnityEngine;
using System.Linq;
using TMPro;

public class TriggerDoor : MonoBehaviour
{
    public string doorID; 
    
    [Header("สถานะ")]
    public bool isLocked = true;

    private Collider2D doorCollider; 
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (textMeshProUGUI != null) textMeshProUGUI.gameObject.SetActive(false);

        // เช็คว่าเคยเปิดไปแล้วหรือยัง (Save System)
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.sceneState.openedDoors.Contains(doorID))
                OpenInstant();
        }
    }

    // ❌ ลบ Update() ทิ้งไปเลย เพราะ Player.cs จะเป็นคนเรียกฟังก์ชันนี้เองเมื่อกด E
    
    public void UnlockAndOpen()
    {
        if (!isLocked) return;

        if (CheckIfPlayerHasKey())
        {
            Debug.Log("ไขกุญแจสำเร็จ: " + doorID);
            
            // ✅ ลบกุญแจออกจากกระเป๋า
            if (Inventory.Instance != null)
            {
                Inventory.Instance.RemoveItem(doorID, 1);
            }

            isLocked = false;
            OpenDoor();

            // ปิด UI ทันทีที่เปิดได้
            if (textMeshProUGUI != null) textMeshProUGUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("ไม่มีกุญแจสำหรับประตู: " + doorID);
            // ใส่เสียงประตู [Locked] ตรงนี้
        }
    }

    bool CheckIfPlayerHasKey()
    {
        if (Inventory.Instance == null) return false;
        return Inventory.Instance.items.Any(item => item.id == doorID);
    }

    public void OpenDoor()
    {
        // บันทึกว่าเปิดแล้ว
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.sceneState.openedDoors.Contains(doorID))
                GameManager.Instance.sceneState.openedDoors.Add(doorID);
        }

        OpenAnimation();
    }

    void OpenInstant()
    {
        isLocked = false;
        if (doorCollider) doorCollider.enabled = false;
        if (spriteRenderer) spriteRenderer.enabled = false;
    }

    void OpenAnimation()
    {
        // ใส่ Animation เปิดประตู / เล่นเสียงเปิด
        if (doorCollider) doorCollider.enabled = false;
        if (spriteRenderer) spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked) return;

        // แสดง UI เมื่อเดินชน (แต่ยังไม่กด E)
        if (collision.CompareTag("Player"))
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.gameObject.SetActive(true);

                if (CheckIfPlayerHasKey())
                    textMeshProUGUI.text = "[E] Unlock";
                else
                    textMeshProUGUI.text = "Locked";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.gameObject.SetActive(false);
            }
        }
    }
}