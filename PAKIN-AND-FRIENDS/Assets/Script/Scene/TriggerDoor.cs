using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    public string doorID;

    void Start()
    {
        if (GameManager.Instance.sceneState.openedDoors.Contains(doorID))
            OpenInstant();
    }

    public void OpenDoor()
    {
        OpenAnimation();
        GameManager.Instance.sceneState.openedDoors.Add(doorID);
    }

    void OpenInstant()
    {
        // ทำให้ประตูเปิดแบบทันที
    }

    void OpenAnimation()
    {
        // เล่นอนิเมชันเปิดประตู
    }
}