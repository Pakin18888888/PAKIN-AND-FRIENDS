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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
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

    // ---------------------------
    // SAVE / LOAD
    // ---------------------------
    public void SaveState()
{
    PlayerPrefs.SetFloat("px", transform.position.x);
    PlayerPrefs.SetFloat("py", transform.position.y);
}

public void LoadState()
{
    float x = PlayerPrefs.GetFloat("px", transform.position.x);
    float y = PlayerPrefs.GetFloat("py", transform.position.y);

    transform.position = new Vector2(x, y);
}
}