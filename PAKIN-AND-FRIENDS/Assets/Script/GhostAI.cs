using System.Collections;
using UnityEngine;

public class GhostAI2D : MonoBehaviour // ✅ สืบทอดจาก MonoBehaviour ถูกต้องแล้ว
{
    // ตัวแปรการไล่ล่าและการตรวจจับ
    public Transform playerTransform;
    public float detectRange = 20f;
    public float speed = 3f;
    public float disappearRange = 30f;
    public float jumpScareDistance = 0.8f;
    public float fadeInDuration = 1.0f; 

    // 🔥 ตัวแปร Jumpscare (ย้ายมาจาก stuff.cs)
    [Header("Jumpscare Settings")]
    public GameObject jumpScareUI;     
    public AudioSource jumpScareSound;  
    public Camera mainCam;              
    public float shakeAmount = 0.2f;
    public float shakeDuration = 0.3f;
    public float jumpScareDisplayTime = 1f; 
    
    // ตัวแปรส่วนตัว
    Rigidbody2D rb;
    bool isJumpScaring = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFadingIn = false;
    private bool isVisible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
            else
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
            
            if (playerTransform == null)
                Debug.LogError("GhostAI2D: NO PLAYER FOUND! Set Player tag correctly.");
        }
        
        // เริ่มต้นให้ผีล่องหน (Alpha = 0)
        if(spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
        }
    }
    
    void FixedUpdate()
    {
        if (playerTransform == null || isJumpScaring || isFadingIn) return; 

        // 🔥 Logic: Player ซ่อนอยู่
        if (Player.Instance != null && Player.Instance.isHiding)
        {
            rb.velocity = Vector2.zero;
            UpdateAnimation(Vector2.zero);

            // ล่องหนทันทีเมื่อ Player ซ่อน
            if (isVisible)
            {
                Color c = spriteRenderer.color;
                c.a = 0f;
                spriteRenderer.color = c;
                isVisible = false;
            }
            return;
        }

        Vector2 ghostPos = rb.position;
        Vector2 playerPos = playerTransform.position;

        float dist = Vector2.Distance(ghostPos, playerPos);

        if (dist <= detectRange)
        {
            // เริ่ม Fade In ถ้ายังไม่โผล่
            if (!isVisible && !isFadingIn)
            {
                StartCoroutine(FadeInGhost());
                return;
            }
            
            // ไล่ตามปกติ
            Vector2 direction = playerPos - ghostPos;
            Vector2 newPos = Vector2.MoveTowards(ghostPos, playerPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            
            UpdateAnimation(direction); 
        }
        else
        {
            // ล่องหนเมื่อหลุดระยะตรวจจับ
            if (isVisible)
            {
                Color c = spriteRenderer.color;
                c.a = 0f;
                spriteRenderer.color = c;
                isVisible = false;
            }

            // ทำลายตัวเองเมื่อหลุดระยะหายตัว
            if (dist >= disappearRange)
            {
                Destroy(gameObject);
            }
        }

        // 🔥 Trigger Jumpscare
        if (isVisible && dist <= jumpScareDistance)
        {
            StartCoroutine(JumpAttack());
        }
    }

    IEnumerator FadeInGhost()
    {
        isFadingIn = true;
        float timer = 0f;
        
        while (timer < fadeInDuration)
        {
            timer += Time.fixedDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
            
            yield return new WaitForFixedUpdate();
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;

        isFadingIn = false;
        isVisible = true;
    }

    void UpdateAnimation(Vector2 direction)
    {
        if (animator == null) return;

        bool isMoving = direction.magnitude > 0.01f;
        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animator.SetFloat("LastInputX", Mathf.Sign(direction.x));
                animator.SetFloat("LastInputY", 0f);
            }
            else
            {
                animator.SetFloat("LastInputX", 0f);
                animator.SetFloat("LastInputY", Mathf.Sign(direction.y));
            }
        }
    }
    
    IEnumerator JumpAttack()
    {
        isJumpScaring = true;
        if (animator != null) animator.SetBool("IsWalking", false);

        // 1) พุ่งเข้าหาผู้เล่น
        float jumpHeight = 1.5f;
        float jumpTime = 0.2f;

        Vector3 startPos = transform.position;
        Vector3 endPos = playerTransform.position;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, endPos, t) + new Vector3(0, height, 0);
            yield return null;
        }

        // 2) เล่น Jumpscare
        if (jumpScareUI != null) jumpScareUI.SetActive(true);
        if (jumpScareSound != null)
            Destroy(gameObject, jumpScareSound.clip.length);
        else
            Destroy(gameObject);


        // 3) Camera Shake
        if (mainCam != null)
        {
            Vector3 originalPos = mainCam.transform.localPosition;
            float shakeTimer = 0f;
            while (shakeTimer < shakeDuration)
            {
                mainCam.transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount;
                shakeTimer += Time.deltaTime;
                yield return null;
            }
            mainCam.transform.localPosition = originalPos;
        }

        // 4) รอและปิด UI
        yield return new WaitForSeconds(jumpScareDisplayTime);
        if (jumpScareUI != null) jumpScareUI.SetActive(false);

        // 5) ผีหายตัว
        Destroy(gameObject);
    }
}