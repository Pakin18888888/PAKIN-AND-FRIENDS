using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
   public static InventoryCanvas Instance; // สร้างตัวแปรกลางไว้เช็ค

    private void Awake()
    {
        // เช็คว่ามี InventoryCanvas ตัวอื่นอยู่ก่อนแล้วหรือยัง
        if (Instance == null)
        {
            // ถ้ายังไม่มี -> ให้ตัวนี้เป็นตัวหลัก
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ถ้ามีอยู่แล้ว (ตัวที่มาจาก Scene ก่อนหน้า) -> ทำลายตัวนี้ทิ้งซะ อย่าให้ซ้ำ
            Destroy(gameObject);
        }
    }
}
