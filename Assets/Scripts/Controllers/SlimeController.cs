using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D),typeof(Ground),typeof(Direction))]
public class SlimeController : MonoBehaviour, IMovementController, IJumpController, IAttackController
{
    // Public variables
	public enum Element {Air,Water,Earth,Fire}
    
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
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _joint;
    private Health _health; 
           
    private Camera _camera;
    
    // Private variables
    [SerializeField] private float hookForce = 20;
    [SerializeField] private float hookRange = 10f;
    [SerializeField] private float fireDashStrength = 20;
    [SerializeField] private float fireDashDuration = .5f;
    [SerializeField] private float wallJumpHeight = 10;
    [SerializeField] private float wallJumpDuration = .2f;
    [SerializeField] private float wallJumpDistance = 10;
    [SerializeField] private float fireBallCooldown = .2f;
    [SerializeField] private float meleeCooldown = .2f;
    [SerializeField] private Vector2 meleeForce;
    [SerializeField] private float fireWaterDamage;
    
    [SerializeField] private GameObject fireball;
    
    private bool _canDash = true;
    private bool _isGrappling = false;
    private bool _canWallJump = true;
    private bool _fireballCooldown = false;
    private bool _canMelee = true;
    
    public Vector2 attackOffset;
    public Vector2 attackSize = new Vector2(1, 2);
   
    private float _movementScale = 1;
    
	private void Start()
	{
		_body = gameObject.GetComponent<Rigidbody2D>();
        _ground = gameObject.GetComponent<Ground>();
        _direction = GetComponent<Direction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jump = GetComponent<Jump>();
        _lineRenderer = GetComponent<LineRenderer>();
        _joint = GetComponent<DistanceJoint2D>();
        _health = GetComponent<Health>();
        
        _camera = Camera.main;
	}

    public float GetMovement()
    {
        return Input.GetAxis("Horizontal") * _movementScale;
    }

    private void Update()
    {
        switch (element)
        {
            case Element.Air:
                _body.mass = 1;
                break;
            case Element.Water:
                _body.mass = 3;
                break;
            case Element.Earth:
                _body.mass = 10;
                break;
            case Element.Fire:
                _body.mass = 3;
                if (_ground.inWater) _health.TakeDamage(fireWaterDamage * Time.deltaTime);
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnJump?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            OnAttack?.Invoke();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
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
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backward
        {
            switch (element)
            {
                case Element.Earth:
                    element = Element.Water;
                    _spriteRenderer.color = Color.blue;
                    break;
                case Element.Fire:
                    element = Element.Earth;
                    _spriteRenderer.color = Color.green;
                    break;
                case Element.Air:
                    element = Element.Fire;
                    _spriteRenderer.color = Color.red;
                    break;
                case Element.Water:
                    element = Element.Air;
                    _spriteRenderer.color = Color.white;
                    break;
            }
        }
        
        if (_ground.onGround)
        {
            _canDash = true;
        }
        
        // Resetting var before switch so anything but air sets it to 0
        _jump.maxAirJumps = 0;
        
        switch (element) {
            // Air element
            case Element.Air:
                // Double jump
                _jump.maxAirJumps = 1;
                break;
            case Element.Water:
                // Wall jump
                if (_ground.wall != 0 && Input.GetKey(KeyCode.Space) && _canWallJump && !_ground.onGround)
                {
                    _canWallJump = false;
                    _body.AddForce(new Vector2(wallJumpDistance * _ground.wall,wallJumpHeight), ForceMode2D.Impulse);
                    StartCoroutine(WallJump());
                } 
                
                // Melee attack
                if (Input.GetMouseButtonDown(0) && _canMelee)
                {
                    
                    LayerMask layerMask = LayerMask.GetMask("Enemy");
                    float k = _direction?.AsSign() ?? 1;
                    Collider2D collider = Physics2D.OverlapBox(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0), attackSize, 0f, layerMask);
                    if (!collider) return;
                    
                    collider.GetComponent<Health>().TakeDamage(1);
                    collider.GetComponent<Rigidbody2D>().AddForce(meleeForce * _direction.AsSign(), ForceMode2D.Impulse);
                    
                    StartCoroutine(Melee());
                }
                break;
            case Element.Earth:
                // Vine hook
                if (Input.GetMouseButtonDown(0))
                {
                    // Get mouse coordinates
                    
                    Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                    
                    RaycastHit2D hit = Physics2D.Raycast( transform.position, (new Vector3(worldPoint.x,worldPoint.y,0) - transform.position).normalized );
                    
                    if ( hit.collider != null && hit.distance <= hookRange)
                    {
                        // Create joint
                        
                        _isGrappling = true;
                        
                        _lineRenderer.enabled = true;
                        
                        GameObject anchor =  new GameObject("Grappling Hook Anchor");
                        
                        anchor.tag = "Anchor";
                        
                        anchor.transform.position = hit.point;
                        Rigidbody2D rb = anchor.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                        rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        _joint.enabled = true;
                        _joint.connectedBody = rb;
                    }
                }

                if (Input.GetMouseButton(0) && _isGrappling)
                {
                    GameObject anchor = GameObject.Find("Grappling Hook Anchor");
                    
                    // Render vine
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, anchor.transform.position);
                    
                    // Pull player
                    _joint.distance -= hookForce * Time.deltaTime;
                    
                    // Break vine if block in the way
                    RaycastHit2D hit = Physics2D.Raycast( transform.position, (new Vector3(anchor.transform.position.x,anchor.transform.position.y,0) - transform.position).normalized );

                    if (new Vector3(hit.point.x,hit.point.y,0) != anchor.transform.position)
                    {
                        _lineRenderer.enabled = false;
                    
                        Destroy(GameObject.Find("Grappling Hook Anchor"));
                        _joint.enabled = false;
                    
                        _isGrappling = false;
                    }
                }
                
                // Stop grapple
                if (Input.GetMouseButtonUp(0) && _isGrappling)
                {
                    _lineRenderer.enabled = false;
                    
                    Destroy(GameObject.Find("Grappling Hook Anchor"));
                    _joint.enabled = false;
                    
                    _isGrappling = false;
                }
                break;
            case Element.Fire:
                // Dash
                if (_canDash && Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(Dash());
                    _body.velocity = new Vector2(0,0);
                    _body.AddForce(new Vector2(_direction.AsSign() * fireDashStrength, 0), ForceMode2D.Impulse);
                    _canDash = false;
                }
                
                // Fireball
                if (Input.GetMouseButtonDown(0) && !_fireballCooldown)
                {
                    Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 directionVector = (worldPoint - (Vector2)transform.position).normalized;
                    
                    float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
            
                    Instantiate(fireball, transform.position + new Vector3(directionVector.x,directionVector.y,0), rotation);
                    StartCoroutine(Fireball());
                }
                break;
        }
    }

    private IEnumerator Melee()
    {
        _canMelee = false;
        yield return new WaitForSeconds(meleeCooldown);
        _canMelee = true;
    }
    
    private IEnumerator WallJump()
    {
        _movementScale = 0;
        yield return new WaitForSeconds(wallJumpDuration);
        _canWallJump = true;
        _movementScale = 1;
    }
    
    private IEnumerator Dash()
    {
        _jump.isDashing = true;
        yield return new WaitForSeconds(fireDashDuration);
        _jump.isDashing = false;
    }

    private IEnumerator Fireball()
    {
        _fireballCooldown = true;
        yield return new WaitForSeconds(fireBallCooldown);
        _fireballCooldown = false;
    }
}
