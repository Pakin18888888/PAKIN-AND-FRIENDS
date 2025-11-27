using UnityEngine;

[System.Serializable]
public class KeyItem : Item
{
    public string unlockKey;      // รหัสกุญแจ
    public bool isQuestItem;      // true = เป็นไอเท็มภารกิจ

    public KeyItem(int id, string name, string key, string desc, bool quest = false)
        : base(id, name, "Key", desc)
    {
        unlockKey = key;
        isQuestItem = quest;
    }

    // ใช้ไอเท็ม — ไม่มีการปลดล็อกใด ๆ ที่นี่
    public override void Use(Player player)
    {
        Debug.Log($"{ItemName} (Key) used. Waiting for door interaction...");
        // ไม่ทำอะไร เพราะระบบประตูจะเป็นคนเช็คว่า player มี key นี้หรือไม่
    }

}