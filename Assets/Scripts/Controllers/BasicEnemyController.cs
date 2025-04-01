using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Direction),typeof(Ground))]
public class BasicEnemyController : MonoBehaviour, IMovementController, IAttackController
{
    public Element element;
    public bool stunned = false;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flipThreshold;
    [SerializeField] private float ledgeDistance;
    [SerializeField] private float attackCooldown;
    
    private Direction _direction;
    private Ground _ground;
	private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    
    private bool _canFlip = false;

    public event Action OnAttack;

    // Old attack "logic"
    //
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         collision.gameObject.GetComponent<Health>().TakeDamage(1);
    //     }
    // }
    
    private void Start()
    {
        StartCoroutine(Attack());
        
        _collider = GetComponent<Collider2D>();
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
        if (stunned) return 0;
        return _ground.onGround ? _direction.AsSign() : 0;
    }

    public IEnumerator Stun(float t)
    {
        stunned = true;
        transform.position += Vector3.forward;
        yield return new WaitForSeconds(t);
        transform.position -= Vector3.forward;
        stunned = false;
    }

    public IEnumerator Attack()
    {
        if (stunned) yield break;
        yield return new WaitForSeconds(attackCooldown);
        OnAttack?.Invoke();
        StartCoroutine(Attack());
    }
}
