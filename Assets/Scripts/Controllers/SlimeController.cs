using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Ground))]
public class SlimeController : MonoBehaviour, IMovementController, IJumpController, IAttackController
{
    // Public variables
	public enum Element {Air,Water,Earth,Fire};
    
	public Element element = Element.Air;

    // Actions from interfaces
    public event Action OnJump;
    public event Action OnAttack;
    
    // Components
	private Rigidbody2D _body;
    private Ground _ground;
    
    // Private variables
    private bool _canDash = true;
    
	private void Start()
	{
		_body = gameObject.GetComponent<Rigidbody2D>();
        _ground = gameObject.GetComponent<Ground>();
	}

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

        if (_ground.onGround)
        {
            _canDash = true;
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            switch (element) {
                case Element.Air:
                    if (_canDash)
                    {
                        _body.velocity = new Vector2(_body.velocity.x, 0);
                        _body.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
                        _canDash = false;
                    }
                    break;
                case Element.Water:
                    // Water movement ability here
                    break;
                case Element.Earth:
                    // Earth's here
                    break;
                case Element.Fire:
                    // Fire's here
                    break;
            }
        }
        
    }
}
