using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _joint;
    
    private Camera _camera;
    
    // Private variables
    [SerializeField] private float hookForce = 20;
    [SerializeField] private float hookRange = 10f;
    [SerializeField] private float fireDashStrength = 20;
    [SerializeField] private float fireDashDuration = .5f;
    
    private bool _canDash = true;
    private bool _isGrappling = false;
    
	private void Start()
	{
		_body = gameObject.GetComponent<Rigidbody2D>();
        _ground = gameObject.GetComponent<Ground>();
        _direction = GetComponent<Direction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jump = GetComponent<Jump>();
        _lineRenderer = GetComponent<LineRenderer>();
        _joint = GetComponent<DistanceJoint2D>();
        
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
            case Element.Air:
                _jump.maxAirJumps = 1;
                break;
            case Element.Water:
                Debug.Log(_ground.wall);
                break;
            case Element.Earth:
                // On click mouse
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
                    
                    Debug.Log(worldPoint);
                    
                    RaycastHit2D hit = Physics2D.Raycast( transform.position, (new Vector3(worldPoint.x,worldPoint.y,0) - transform.position).normalized );
                    
                    if ( hit.collider != null && hit.distance <= hookRange )
                    {
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

                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, anchor.transform.position);
                    
                    _joint.distance -= hookForce * Time.deltaTime;
                    
                    RaycastHit2D hit = Physics2D.Raycast( transform.position, (new Vector3(anchor.transform.position.x,anchor.transform.position.y,0) - transform.position).normalized );

                    if (new Vector3(hit.point.x,hit.point.y,0) != anchor.transform.position)
                    {
                        _lineRenderer.enabled = false;
                    
                        Destroy(GameObject.Find("Grappling Hook Anchor"));
                        _joint.enabled = false;
                    
                        _isGrappling = false;
                    }
                }
                
                if (Input.GetMouseButtonUp(0) && _isGrappling)
                {
                    _lineRenderer.enabled = false;
                    
                    Destroy(GameObject.Find("Grappling Hook Anchor"));
                    _joint.enabled = false;
                    
                    _isGrappling = false;
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
