using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<string> items = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public List<string> GetItemIdList()
    {
        return new List<string>(items);
    }

    public void LoadFromIdList(List<string> saved)
    {
        items = new List<string>(saved);
    }
}
