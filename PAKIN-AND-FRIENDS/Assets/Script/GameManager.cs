using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData player;
    public SceneStateData sceneState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            player = new PlayerData();
            sceneState = new SceneStateData();
        }
        else Destroy(gameObject);
    }
}