using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identity : MonoBehaviour
{
    public string Name;
    public int PositionX;
    public int PositionY;
    void Start()
    {
        SetupIdentity();
    }
    protected virtual void SetupIdentity()
    {
        transform.position = new Vector3(PositionX, transform.position.y,0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}