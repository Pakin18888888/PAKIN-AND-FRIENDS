using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("ID กุญแจ เช่น key_blue")]
    public string keyID;
    public Sprite icon;

    void Start()
    {
        if (Inventory.Instance != null &&
            Inventory.Instance.HasItem(keyID))
        {
            Destroy(gameObject);
        }
    }

    public void OnInteract()
    {
        Inventory.Instance.AddItem(keyID, icon, 1);

        Debug.Log("เก็บกุญแจ " + keyID);

        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return "เก็บกุญแจ " + keyID;
    }
}
