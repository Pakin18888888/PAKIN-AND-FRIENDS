using System.Collections;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("Detect & Move")]
    public Transform playerTransform;
    public float detectRange = 20f;
    public float speed = 3f;
    public float disappearRange = 30f;
    public float jumpScareDistance = 0.8f;

    [Header("Fade Settings")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Hide Logic")]
    public float hideDelayBeforeFade = 1.5f;   // ดีเลย์ก่อนเริ่มเฟด
    public float hideTimeToDisappear = 3f;     // เวลาที่ต้องอยู่ในตู้ถึงจะหาย

    [Header("Jumpscare")]
    public GameObject jumpScareUI;
    public AudioSource jumpScareSound;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isVisible = false;
    bool isFadingIn = false;
    bool isFadingOut = false;
    bool isJumpScaring = false;

    float hideTimer = 0f;
    float hideDelayTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform == null)
        {
            if (Player.Instance != null)
                playerTransform = Player.Instance.transform;
            else
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        SetAlpha(0f);
    }

    void FixedUpdate()
    {
        if (playerTransform == null || isJumpScaring)
            return;

        // ================= PLAYER HIDING =================
        if (Player.Instance != null && Player.Instance.isHiding)
        {
            rb.velocity = Vector2.zero;
            UpdateAnimation(Vector2.zero);

            // ✅ กันไม่ให้ผี fade-in / โผล่ตอนผู้เล่นอยู่ในตู้
            hideDelayTimer += Time.fixedDeltaTime;

            if (hideDelayTimer >= hideDelayBeforeFade)
            {
                hideTimer += Time.fixedDeltaTime;

                if (hideTimer >= hideTimeToDisappear && !isFadingOut)
                {
                    StartCoroutine(FadeOutAndDisappear());
                }
            }

            return;
        }
        else
        {
            hideTimer = 0f;
            hideDelayTimer = 0f;
        }

        Vector2 ghostPos = rb.position;
        Vector2 playerPos = playerTransform.position;
        float dist = Vector2.Distance(ghostPos, playerPos);

        // ================= DETECT PLAYER =================
        if (dist <= detectRange)
        {
            if (!isVisible && !isFadingIn)
                StartCoroutine(FadeInGhost());

            Vector2 direction = playerPos - ghostPos;
            Vector2 newPos = Vector2.MoveTowards(ghostPos, playerPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            UpdateAnimation(direction);
        }
        else
        {
            if (isVisible)
            {
                SetAlpha(0f);
                isVisible = false;
            }

            if (dist >= disappearRange)
                Destroy(gameObject);
        }

        // ================= JUMPSCARE =================
        if (isVisible && dist <= jumpScareDistance && !isJumpScaring)
        {
            StartCoroutine(JumpAttack());
        }
    }

    // ================= FADE IN =================
    IEnumerator FadeInGhost()
    {
        isFadingIn = true;

        float t = 0f;
        while (t < fadeInDuration)
        {
            // ❌ ถ้าผู้เล่นเข้าตู้ระหว่างเฟด → หยุดทันที
            if (Player.Instance != null && Player.Instance.isHiding)
            {
                isFadingIn = false;
                yield break;
            }

            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, t / fadeInDuration));
            yield return null;
        }

        SetAlpha(1f);
        isVisible = true;
        isFadingIn = false;
    }

    // ================= FADE OUT =================
    IEnumerator FadeOutAndDisappear()
    {
        isFadingOut = true;
        StopAllCoroutines();

        float t = 0f;
        float startAlpha = spriteRenderer.color.a;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, 0f, t / fadeOutDuration));
            yield return null;
        }

        DisappearForever();
    }

    // ================= JUMPSCARE =================
    IEnumerator JumpAttack()
    {
        isJumpScaring = true;

        if (jumpScareUI != null) jumpScareUI.SetActive(true);
        if (jumpScareSound != null) jumpScareSound.Play();

        yield return new WaitForSeconds(0.5f);
        DisappearForever();
    }

    // ================= UTILS =================
    void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        bool move = dir.magnitude > 0.01f;
        animator.SetBool("IsWalking", move);

        if (move)
        {
            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
        }
    }

    void SetAlpha(float a)
    {
        if (spriteRenderer == null) return;
        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }

    void DisappearForever()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
