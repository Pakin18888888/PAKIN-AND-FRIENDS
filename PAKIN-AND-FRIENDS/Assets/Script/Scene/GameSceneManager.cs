using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeScene(string next)
    {
        FindObjectOfType<Player>()?.SaveState();
        SceneManager.LoadScene(next);
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene s, LoadSceneMode mode)
    {
        FindObjectOfType<Player>()?.LoadState();
    }
}