using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerLocker : MonoBehaviour
{
    private GameObject player;
    public BoxCollider2D boxCollider2D;
    public TextMeshProUGUI promptText;

    private bool playerInRange = false;
    private bool isLocked = false; // สถานะว่ามีคนซ่อนอยู่ไหม

    public Transform hidePoint;
    public Transform exitPoint;

    void Start()
    {
        if (Player.Instance != null)
        {
            player = Player.Instance.gameObject;
        }

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void UpdatePrompt()
    {
        if (!promptText) return;

        // ถ้าล็อคอยู่ ให้โชว์ปุ่มตลอด (เพื่อให้กดออกได้) หรือถ้าอยู่ในระยะก็โชว์
        if (!playerInRange && !isLocked) 
        {
            promptText.gameObject.SetActive(false);
            return;
        }

        promptText.gameObject.SetActive(true);
        promptText.text = isLocked ? "[E] Unlock" : "[E] Lock";
    }

    void ToggleLock()
    {
        isLocked = !isLocked;

        if (Player.Instance != null)
        {
            Player.Instance.SetMovement(!isLocked);
            SpriteRenderer sp = player.GetComponent<SpriteRenderer>();

            if (isLocked)
            {
                // เข้าตู้
                if (hidePoint != null)
                    Player.Instance.transform.position = hidePoint.position;
                sp.enabled = false;
                boxCollider2D.enabled = false;
            }
            else
            {
                // ออกตู้
                if (exitPoint != null)
                    Player.Instance.transform.position = exitPoint.position;
                sp.enabled = true;
                boxCollider2D.enabled = true;
                
                // 🔥 เพิ่มบรรทัดนี้: เมื่อออกมาแล้ว ถ้าจุดออกอยู่นอก Collider ให้รีเซ็ตค่าทิ้งเลย
                // เพื่อป้องกันบั๊กกดซ้ำ
                // แต่ปกติ OnTriggerExit จะทำงานให้อัตโนมัติเมื่อเดินออกไป
            }
        }

        UpdatePrompt();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            UpdatePrompt();

            if (Player.Instance != null)
            {
                Player.Instance.currentLocker = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // 🔥🔥🔥 จุดที่แก้: ถ้ากำลังซ่อนอยู่ (isLocked = true) ห้ามยกเลิกค่า!
            // เพราะบางที HidePoint มันอาจจะเผลอหลุดขอบ Collider ไปนิดนึง
            if (isLocked) return; 

            playerInRange = false;
            UpdatePrompt();

            if (Player.Instance != null && Player.Instance.currentLocker == this)
            {
                Player.Instance.currentLocker = null;
            }
        }
    }

    public void OnPlayerInteracting()
    {
        // ยอมให้กดได้ ถ้าอยู่ในระยะ หรือ ถ้าถูกขังอยู่ข้างใน
        if (playerInRange || isLocked)
            ToggleLock();
    }
}