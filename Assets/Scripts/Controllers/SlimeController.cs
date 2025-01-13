using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IMovementController, IJumpController
{
    public event Action OnJump;

    public float GetMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnJump?.Invoke();
        } 
    }
}
