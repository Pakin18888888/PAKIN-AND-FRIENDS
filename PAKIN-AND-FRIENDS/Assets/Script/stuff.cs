using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class stuff : Identity
{
    public TMP_Text interactionTextUI;
    protected Collider2D _collider;
    public bool isHide;

    protected override void SetupIdentity()
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
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        return Vector2.Distance(transform.position, player.position);
    }
}
