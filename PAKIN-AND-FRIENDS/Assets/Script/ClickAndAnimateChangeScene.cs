using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 1. เพิ่ม , IInteractable ต่อท้าย MonoBehaviour
public class ClickAndAnimateChangeScene : MonoBehaviour, IInteractable
{
    public Animator animator;      
    public string stateName;       
    public string nextSceneName;
    [TextArea]
    public string descriptionText = "ตรวจสอบ";

    private bool clicked = false;

    // ลบ OnMouseDown ออก แล้วเปลี่ยนเป็นฟังก์ชันนี้แทน
    // ฟังก์ชันนี้จะถูกเรียกโดย Player เมื่อคลิกโดนและอยู่ในระยะ
    public void OnInteract()
    {
        if (clicked) return; // ป้องกันการกดรัว
        clicked = true;

        Debug.Log("Player คลิกโดน object นี้แล้ว!");

        // เล่น State จาก Animator
        if (animator != null)
        {
            animator.Play(stateName);
            StartCoroutine(WaitAndChangeScene());
        }
        else
        {
            // ถ้าลืมใส่ Animator ให้เปลี่ยนซีนเลยกันค้าง
            SceneManager.LoadScene(nextSceneName);
        }
    }
    public string GetDescription()
    {
        return descriptionText;
    }

    IEnumerator WaitAndChangeScene()
    {
        // รอ 1 เฟรมเพื่อให้ Animator เริ่มเล่นก่อน ถึงจะดึงเวลาได้ถูกต้อง
        yield return null; 

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // รอจนกว่าคลิปจะเล่นจบ
        yield return new WaitForSeconds(info.length);

        SceneManager.LoadScene(nextSceneName);
    }
}