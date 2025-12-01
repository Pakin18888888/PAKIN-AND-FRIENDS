using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TriggerLocker : MonoBehaviour
{
    private GameObject player;          // Player ตัวจริงใน Hierarchy
    public TextMeshProUGUI promptText; // Text "Press E"

    private bool playerInRange = false;
    private bool isHidden = false;
  

    void Start()
    {
        player = Player.Instance.gameObject; 
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleHide();
        }
    }

    void ToggleHide()
    {
        isHidden = !isHidden;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        Animator anim = player.GetComponent<Animator>();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (isHidden)
        {
            // ซ่อน Player
            if (sr) sr.enabled = false;
            if (anim) anim.enabled = false;
            if (rb) rb.velocity = Vector2.zero;

            if (promptText)
                promptText.text = "[E] Reveal";
        }
        else
        {
            // แสดง Player กลับ
            if (sr) sr.enabled = true;
            if (anim) anim.enabled = true;

            if (promptText)
                promptText.text = "[E] Hide";
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            playerInRange = true;

            if (promptText)
            {
                promptText.text = "[E] Hide";
                promptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            playerInRange = false;

            if (promptText)
                promptText.gameObject.SetActive(false);
        }
    }

    public void OnPlayerInteracting()
    {
        if(playerInRange)
        {
            ToggleHide();
        }
    }

   
}
