using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public static Player Instance;
    public LayerMask InteractLayer;

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

        rb.velocity = moveInput * moveSpeed;
    }

    // Input System
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 current = context.ReadValue<Vector2>();
        moveInput = current;

        animator.SetBool("IsWalking", current.magnitude > 0);
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

       


        if (context.canceled)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    // กด E
    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");

        if (!context.started) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f, InteractLayer);

        Debug.Log("HIT COUNT = " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("HIT OBJECT = " + hit.name);

            TriggerLocker locker = hit.GetComponent<TriggerLocker>();

            if (locker != null)
            {
                Debug.Log("FOUND TriggerLocker");
                locker.OnPlayerInteracting();
                break;
            }
        }
    }
}