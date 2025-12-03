using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    // Start is called before the first frame update
    public static JumpScare Instance; // ตัวแปรนี้จะเข้าถึงได้จากทุก Script

    public GameObject jumpScareUI;      // UI รูปผี
    public AudioSource jumpScareSound;  // เสียง jumpscare
    public Camera mainCam;              // กล้อง 2D
    public float shakeAmount = 0.2f;    // ความแรงการสั่น
    public float shakeDuration = 0.3f;  // ระยะเวลาสั่น
    public float jumpScareDisplayTime = 1.0f; // ระยะเวลาโชว์รูปผี
public void PlayJumpScare()
    {
        StartCoroutine(JumpScareRoutine());
    }

void Awake() 
    {
        Instance = this;
        
    }
IEnumerator JumpScareRoutine()
{
    
    // เปิดภาพผี
    if (jumpScareUI != null)
        jumpScareUI.SetActive(true);

    // เล่นเสียง
    if (jumpScareSound != null)
        jumpScareSound.Play();

    // สั่นกล้อง
    Vector3 originalPos = mainCam.transform.localPosition;
    float time = 0f;

    while (time < shakeDuration)
    {
        mainCam.transform.localPosition =
            originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;

        time += Time.deltaTime; // <<< ไม่หยุดเกม
        yield return null;
    }

    // คืนตำแหน่งกล้อง
    mainCam.transform.localPosition = originalPos;

     // 4) รอจนรูปผีจะต้องปิดเอง
    yield return new WaitForSeconds(jumpScareDisplayTime);
    

    // 5) ปิดรูปผี
    if (jumpScareUI != null)
        jumpScareUI.SetActive(false);
        Debug.Log("JUMPSCARE: END");
}
}