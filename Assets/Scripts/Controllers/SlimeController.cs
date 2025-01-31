using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D),typeof(Ground),typeof(Direction))]
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
    private Direction _direction;
    private SpriteRenderer _spriteRenderer;
    private Jump _jump;
    
    // Private variables
    [SerializeField] private float airDashStrength = 20;
    [SerializeField] private float fireDashStrength = 20;
    [SerializeField] private float fireDashDuration = .5f;
    
    private bool _canDash = true;
    
	private void Start()
	{
		_body = gameObject.GetComponent<Rigidbody2D>();
        _ground = gameObject.GetComponent<Ground>();
        _direction = GetComponent<Direction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jump = GetComponent<Jump>();
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            switch (element)
            {
                case Element.Air:
                    element = Element.Water;
                    _spriteRenderer.color = Color.blue;
                    break;
                case Element.Water:
                    element = Element.Earth;
                    _spriteRenderer.color = Color.green;
                    break;
                case Element.Earth:
                    element = Element.Fire;
                    _spriteRenderer.color = Color.red;
                    break;
                case Element.Fire:
                    element = Element.Air;
                    _spriteRenderer.color = Color.white;
                    break;
            }
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
                        _body.AddForce(new Vector2(0, airDashStrength), ForceMode2D.Impulse);
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
                    if (_canDash)
                    {
                        StartCoroutine(Dash());
                        _body.velocity = new Vector2(0,0);
                        _body.AddForce(new Vector2(_direction.AsSign() * fireDashStrength, 0), ForceMode2D.Impulse);
                        _canDash = false;
                    }
                    break;
            }
        }
    }

    private IEnumerator Dash()
    {
        _jump.isDashing = true;
        yield return new WaitForSeconds(fireDashDuration);
        _jump.isDashing = false;
    } 
}
