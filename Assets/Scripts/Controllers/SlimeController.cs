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
    
    private Camera _camera;
    
    // Private variables
    [SerializeField] private float vineHookForce = 20;
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
        
        _camera = Camera.main;
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

        if (element == Element.Water)
        {
            _jump.isDashing = true;
        }
        else
        {
            _jump.isDashing = false;
        }
        
        // Resetting var before switch so anything but air sets it to 0
        _jump.maxAirJumps = 0;
        
        switch (element) {
            case Element.Air:
                _jump.maxAirJumps = 1;
                break;
            case Element.Water:
                // Water movement ability here
                break;
            case Element.Earth:
                // On click mouse
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast( transform.position, worldPoint );
                    if ( hit.collider != null )
                    {
                        GameObject anchor =  new GameObject("Grappling Hook Anchor");

                        anchor.tag = "Anchor";
                        
                        anchor.transform.position = hit.point;
                        Rigidbody2D rb = anchor.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                        rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        DistanceJoint2D joint = anchor.AddComponent(typeof(DistanceJoint2D)) as DistanceJoint2D;
                        joint.connectedBody = _body;
                        
                        _body.velocity = Vector2.zero;
                        _body.AddForce(new Vector2(vineHookForce * _direction.AsSign(), 0),ForceMode2D.Impulse);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Destroy(GameObject.FindWithTag("Anchor"));
                }
                break;
            case Element.Fire:
                if (_canDash && Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Dash());
                    _body.velocity = new Vector2(0,0);
                    _body.AddForce(new Vector2(_direction.AsSign() * fireDashStrength, 0), ForceMode2D.Impulse);
                    _canDash = false;
                }
                break;
        }
    }

    private IEnumerator Dash()
    {
        _jump.isDashing = true;
        yield return new WaitForSeconds(fireDashDuration);
        _jump.isDashing = false;
    } 
}
