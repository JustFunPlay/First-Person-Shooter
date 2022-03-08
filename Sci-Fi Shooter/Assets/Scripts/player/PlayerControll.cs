using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : CharacterHealth
{
    Rigidbody rb;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction inputAction)
    {
        
    }
    public void OnLook(InputAction inputAction)
    {

    }
    public void OnFire(InputAction inputAction)
    {

    }
    public void OnAltFire(InputAction inputAction)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
