using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // จำเป็นต้องใช้สำหรับ Keyboard.current

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
    
    public bool canMove = true;

    // ตัวแปรจำว่ายืนหน้าตู้ไหน
    [HideInInspector] public TriggerLocker currentLocker;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Instance != this) return;

        // 1. ส่วนการเดิน (Move)
        if (canMove)
            rb.velocity = moveInput * moveSpeed;
        else
            rb.velocity = Vector2.zero;

        UpdateAnimation();

        // 2. ส่วนการกด E (Interact) - เขียนเช็กตรงนี้เลยครับ
        // Keyboard.current.eKey.wasPressedThisFrame มีค่าเท่ากับ Input.GetKeyDown(KeyCode.E) แต่ใช้กับระบบใหม่
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) 
        {
            TryInteract();
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = moveInput.magnitude > 0;
        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    // ฟังก์ชันรับค่าเดินจาก Input System (ไม่ต้องแก้)
    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            moveInput = Vector2.zero;
    }

    // ฟังก์ชันสั่งงานตู้ (แยกออกมาเพื่อให้เรียกใช้ง่ายๆ)
    public void TryInteract()
    {
        // ถ้าเรายืนอยู่หน้าตู้ (ตัวแปรไม่เป็น null) ให้สั่งงานตู้นั้นเลย
        if (currentLocker != null)
        {
            currentLocker.OnPlayerInteracting();
        }
    }

    public void SetMovement(bool status)
    {
        canMove = status;

        if (!status)
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }
    }
}