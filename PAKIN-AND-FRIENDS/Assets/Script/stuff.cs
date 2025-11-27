using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class stuff : Identity
{
    public TMP_Text interactionTextUI;
    protected Collider2D _collider;
    public bool isHide;

    // เพิ่มตัวแปรเก็บ Player ไว้ เพื่อไม่ต้องหาใหม่ทุกรอบ
    private Transform playerTransform; 

    protected override void SetupIdentity()
    {
        interactionTextUI = GetComponentInChildren<TMP_Text>();
        _collider = GetComponent<Collider2D>();

        // 1. ค้นหา Player แค่ครั้งเดียวตรงนี้
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("หา Player ไม่เจอ! เช็ก Tag หรือยังครับ?");
        }
        
        // ซ่อนข้อความไว้ก่อนตอนเริ่ม
        if(interactionTextUI != null) interactionTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        // ถ้าไม่มี Player (เช่น ตาย หรือยังไม่เกิด) ให้ข้ามไปเลย กัน Error
        if (playerTransform == null) return;

        // 2. ใช้ตัวแปรที่เก็บไว้ แทนการค้นหาใหม่
        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist > 2f)
        {
            if (interactionTextUI.gameObject.activeSelf) 
                interactionTextUI.gameObject.SetActive(false);
        }
        else
        {
            if (!interactionTextUI.gameObject.activeSelf) 
                interactionTextUI.gameObject.SetActive(true);

            // 3. (เสริม) เช็กการกดปุ่ม เช่น กด E เพื่อโต้ตอบ
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("กดปุ่มโต้ตอบแล้ว!");
                // ใส่ฟังก์ชันทำงานตรงนี้ เช่น เปิดประตู, เก็บของ
            }
        }
    }

    // ฟังก์ชันนี้ไม่จำเป็นต้องมีแยกแล้ว เพราะเขียนรวมใน Update ได้เลยเพื่อความเร็ว
}