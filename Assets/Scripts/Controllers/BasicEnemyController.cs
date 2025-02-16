using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Direction),typeof(Ground))]
public class BasicEnemyController : MonoBehaviour, IMovementController
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flipThreshold;

    private Direction _direction;
    private Ground _ground;
	private Rigidbody2D _body;
    
    private bool _canFlip = false;

    private void Start()
    {
        _direction = GetComponent<Direction>();
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
    }

    private void Update()
    {
        if (Mathf.Abs(_body.velocity.x) > flipThreshold)
        {
            _canFlip = true;
        }
        
        if (Mathf.Abs(_body.velocity.x) < flipThreshold && _canFlip)
        {
            _direction.Flip();
            _canFlip = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.right * _direction.AsSign()),Vector2.down);
        
        if (hit.distance > 1) _direction.Flip();
    }

    public float GetMovement()
    {
        return _ground.onGround ? _direction.AsSign() : 0;
    }
}
