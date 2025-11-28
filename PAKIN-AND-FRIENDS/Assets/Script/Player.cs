using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public int hp = 100;
    public int flashlightBattery = 100;

    public Inventory inventory;
    public static Player Instance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

   void Update()
    {
        // เช็ค Instance เพื่อกัน Error ถ้าตัวนี้ถูกทำลายไปแล้ว
        if (Instance != this) return; 
        
        rb.velocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 currentMoveInput = context.ReadValue<Vector2>();

        animator.SetBool("IsWalking", currentMoveInput.magnitude > 0);

        if (context.canceled)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        
        moveInput = currentMoveInput;

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

void Awake()
    {
        // ระบบ Singleton: ถ้ามี Player อยู่แล้ว ให้ทำลายตัวนี้ทิ้ง (ป้องกันตัวซ้ำ)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }

        // ถ้ายังไม่มี ให้ตัวนี้เป็นตัวหลัก และห้ามทำลายเมื่อเปลี่ยนฉาก
        Instance = this;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
}