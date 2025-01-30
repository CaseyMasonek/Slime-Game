using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IMovementController, IJumpController, IMovementSpecialController, IAttackController
{
	public enum Element {Air,Water,Earth,Fire};
	
	public Element element = Element.Air;

    public event Action OnJump;
    public event Action OnSpecialMove;
    public event Action OnAttack;
    
    public float GetMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnJump?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnAttack?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnSpecialMove?.Invoke();
        }
    }
}
