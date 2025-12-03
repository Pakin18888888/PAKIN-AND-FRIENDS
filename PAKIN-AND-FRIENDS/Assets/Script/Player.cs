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
    
    public bool canMove = true;

    // 🔥 ตัวแปรสถานะการซ่อน
    [HideInInspector] public bool isHiding = false; 

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

        if (canMove)
            rb.velocity = moveInput * moveSpeed;
        else
            rb.velocity = Vector2.zero;

        UpdateAnimation();

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

    public void TryInteract()
    {
        if (currentLocker != null)
        {
            currentLocker.OnPlayerInteracting();
        }
    }

    public void SetMovement(bool status)
    {
        canMove = status;
        
        // 🔥 ควบคุมสถานะ isHiding
        isHiding = !status; 

        if (!status)
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }
    }
}