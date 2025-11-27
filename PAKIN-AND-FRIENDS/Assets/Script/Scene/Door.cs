using UnityEngine;

public class Door : MonoBehaviour
{

    public string scene;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            FindObjectOfType<GameSceneManager>()
                .ChangeScene(scene);
        }
    }
}