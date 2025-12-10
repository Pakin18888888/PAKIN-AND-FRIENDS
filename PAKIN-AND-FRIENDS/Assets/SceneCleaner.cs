using UnityEngine;

public class SceneCleaner : MonoBehaviour
{
    void Start()
    {
        GameObject p = GameObject.Find("Player");
        if (p != null) Destroy(p);

        GameObject  f = GameObject.Find("Particle System");
        if (f != null) Destroy(f);

        GameObject  b = GameObject.Find("GlobalAudioManager");
        if (b != null) Destroy(b);
        GameObject  b1 = GameObject.Find("GameOverUI");
        if (b1 != null) Destroy(b1);

        GameObject  gamemaneger = GameObject.Find("GameManeger");
        if (gamemaneger != null) Destroy(gamemaneger);

        GameObject inv = GameObject.Find("Canvas_Inventory");
        if (inv != null) Destroy(inv);

        GameObject sanity = GameObject.Find("SanityCanvas");
        if (sanity != null) Destroy(sanity);
    }
}