using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;

    public void ShowButton()
    {
        startButton.SetActive(true);
    }

    void Start()
    {
        startButton.SetActive(false); // ซ่อนปุ่มก่อน
    }
}

