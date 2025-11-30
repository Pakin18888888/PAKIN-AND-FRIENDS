using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class stuff : MonoBehaviour
{
    public TextMeshProUGUI GJ;
    public GameObject GJ1;
    public bool pickUpAllowed;

     float GetDistancePlayer()
    {
        // สมมติ Player มี tag ว่า "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return Mathf.Infinity;

        return Vector3.Distance(player.transform.position, transform.position);
    }

    void Start()
    {
        GJ.gameObject.SetActive(false);
        GJ1.gameObject.SetActive(false);
        
        pickUpAllowed = false;
    }

    void Update()
    {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E)){

            bool isActive = !GJ1.activeSelf;
            bool isActive1 = !GJ.gameObject.activeSelf;
            GJ.gameObject.SetActive(isActive1);
            GJ1.SetActive(isActive);

            if (isActive)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GJ.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(GJ != null)
            {
                GJ.gameObject.SetActive(false);
            }
            pickUpAllowed = false;
        }
    }
}