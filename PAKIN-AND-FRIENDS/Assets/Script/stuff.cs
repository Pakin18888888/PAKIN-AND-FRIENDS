using UnityEngine;
using TMPro;

public abstract class stuff : Identity
{
    public TMP_Text interactionTextUI;
    protected Collider2D _collider;
    public bool isHide;

    public void Setup()
    {
        interactionTextUI = GetComponentInChildren<TMP_Text>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (GetDistancePlayer() > 2f)
        {
            interactionTextUI.gameObject.SetActive(false);
        }
        else
        {
            interactionTextUI.gameObject.SetActive(true);
        }
    }

    float GetDistancePlayer()
    {
        // คุณต้องแก้ส่วนนี้ให้ถูกตามโปรเจคคุณ
        // ตัวอย่าง: หา Player จาก tag
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        return Vector3.Distance(transform.position, player.position);
    }
}