using UnityEngine;

public class FlashlightItem : MonoBehaviour, IInteractable
{
    public string itemID = "flashlight";
    public Sprite icon;

    void Start()
    {
        if (Inventory.Instance != null &&
            Inventory.Instance.HasItem(itemID))
        {
            Destroy(gameObject);
        }
    }

    public void OnInteract()
    {
        Inventory.Instance.AddItem(itemID, icon, 1);

        if (Player.Instance != null)
            Player.Instance.EnableFlashlight();

        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return "เก็บไฟฉาย";
    }
}
