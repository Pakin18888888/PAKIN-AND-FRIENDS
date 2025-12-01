using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerLocker : MonoBehaviour
{
    private GameObject player;
    public TextMeshProUGUI promptText;

    private bool playerInRange = false;
    private bool isLocked = false;

    // จุดซ่อน + จุดออกจากตู้
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

        if (!playerInRange)
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

        // ล็อกตัวผู้เล่น
        if (Player.Instance != null)
        {
            Player.Instance.SetMovement(!isLocked);
            SpriteRenderer sp = player.GetComponent<SpriteRenderer>();

            if (isLocked)
            {
                // เข้าไปในตู้
                if (hidePoint != null)
                    Player.Instance.transform.position = hidePoint.position;
                    sp.enabled = false;
            }
            else
            {
                // ออกจากตู้
                if (exitPoint != null)
                    Player.Instance.transform.position = exitPoint.position;
                    sp.enabled = true;
            }
        }

        UpdatePrompt();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // เช็คว่าสิ่งที่ชนคือ Player
        if (col.gameObject.CompareTag("Player") || (player != null && col.gameObject == player))
        {
            playerInRange = true;
            UpdatePrompt();

            // บอก Player ว่า "ฉันคือตู้ที่เธออยู่ใกล้นะ"
            if (Player.Instance != null)
            {
                Player.Instance.currentLocker = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // เช็คว่าสิ่งที่ชนคือ Player
        if (col.gameObject.CompareTag("Player") || (player != null && col.gameObject == player))
        {
            playerInRange = false;
            UpdatePrompt();

            // เมื่อเดินออก ให้ล้างค่าใน Player ทิ้ง (ถ้าตู้นั้นยังเป็นตู้นี้อยู่)
            if (Player.Instance != null && Player.Instance.currentLocker == this)
            {
                Player.Instance.currentLocker = null;
            }
        }
    }

    public void OnPlayerInteracting()
    {
        if (playerInRange)
            ToggleLock();
    }
}