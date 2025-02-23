using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Direction),typeof(Ground))]
public class BasicEnemyController : MonoBehaviour, IMovementController
{
    public Element element;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flipThreshold;
    [SerializeField] private float ledgeDistance;

    private Direction _direction;
    private Ground _ground;
	private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    
    private bool _canFlip = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }
    
    private void Start()
    {
        _direction = GetComponent<Direction>();
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        switch (element)
        {
            case Element.None:
                _spriteRenderer.color = Color.white;
                break;
            case Element.Water:
                _spriteRenderer.color = Color.blue;
                break;
            case Element.Earth:
                _spriteRenderer.color = Color.green;
                break;
            case Element.Fire:
                _spriteRenderer.color = Color.red;
                break;
            case Element.Air:
                _spriteRenderer.color = Color.gray;
                break;
        }
    }

    private void Update()
    {
        /*
        if (Mathf.Abs(_body.velocity.x) > flipThreshold)
        {
            _canFlip = true;
        }
        
        if (Mathf.Abs(_body.velocity.x) < flipThreshold && _canFlip)
        {
            _direction.Flip();
            _canFlip = false;
        }
        */

        RaycastHit2D hit = Physics2D.Raycast(transform.position + (ledgeDistance * _direction.AsSign() * Vector3.right),Vector2.down);
        
        if (hit.distance > 1) _direction.Flip();
    }

    public float GetMovement()
    {
        return _ground.onGround ? _direction.AsSign() : 0;
    }
}
