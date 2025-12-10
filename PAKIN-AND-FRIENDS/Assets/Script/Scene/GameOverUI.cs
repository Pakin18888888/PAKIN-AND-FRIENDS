using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [Header("UI Panel")]
    public GameObject gameOverPanel;

    [Header("Sound")]
    public AudioSource gameOverSound;   // ลากเสียง Game Over ใส่
    public bool destroyExistingUI = true;
    public bool destroyAudioObjects = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // ===========================================
    // เรียกตอนเกิด Game Over
    // ===========================================
    public void ShowGameOver()
    {
        Debug.Log("📌 GAMEOVER UI SHOWED");

        // 1. ลบ UI ซ้ำซ้อน (ถ้ามี)
        if (destroyExistingUI)
            RemoveDuplicateUI();

        // 2. ปิดเสียงเก่าที่ค้าง (ถ้ามี)
        if (destroyAudioObjects)
            RemoveAudioObjects();

        // 3. แสดง Panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 4. เล่นเสียง GameOver
        if (gameOverSound != null)
            gameOverSound.Play();
    }

    // ===========================================
    // ปุ่ม Restart
    // ===========================================
    public void RestartGame()
    {
        Debug.Log("🔄 Restarting...");

        SceneManager.LoadScene("StartScene");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ===========================================
    // ปุ่ม Quit
    // ===========================================
    public void QuitGame()
    {
        Application.Quit();
    }

    // ===========================================
    // ล้าง UI ซ้ำใน DontDestroyOnLoad
    // ===========================================
    void RemoveDuplicateUI()
    {
        GameOverUI[] all = FindObjectsOfType<GameOverUI>();

        foreach (var ui in all)
        {
            if (ui != Instance)
            {
                Debug.Log("🗑 ลบ GameOverUI ซ้ำ: " + ui.gameObject.name);
                Destroy(ui.gameObject);
            }
        }
    }

    // ===========================================
    // ล้าง Object เสียงที่พกมาหลายอัน
    // ===========================================
    void RemoveAudioObjects()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();

        foreach (var src in sources)
        {
            if (src.gameObject != this.gameObject &&
                src != gameOverSound)
            {
                Debug.Log("🗑 ลบเสียงที่ไม่จำเป็น: " + src.gameObject.name);
                Destroy(src.gameObject);
            }
        }
    }
}
