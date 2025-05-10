using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class MinibossController : MonoBehaviour, IMovementController, IJumpController
{
    [SerializeField] private float cooldown;
    
    private GameObject _player;
    private Direction _direction;
    private Animator _animator;

    public event Action OnJump;

    private Vector2 attackSize = new Vector2(3, 1);
    private Vector2 attackOffset = new Vector2(2, -2);
    private Vector2 raycastOffset = new Vector2(2.5f, -1.2f);
    [SerializeField] private float attackTimeOffset = 1;
    
    private bool _attacking;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _direction = GetComponent<Direction>();
        _animator = GetComponent<Animator>();

        StartCoroutine(AttackLoop());
    }

    private void Update()
    {
        if (_player.transform.position.x > transform.position.x != Mathf.Approximately(_direction.AsSign(), 1))
        {
            _direction.Flip();
        }
        
        LayerMask mask = LayerMask.GetMask("Ground");
        float k = _direction?.AsSign() ?? 1;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(raycastOffset.x * k, raycastOffset.y, 0),
            Vector2.right * _direction.AsSign(), mask);

        Debug.DrawRay(transform.position + new Vector3(raycastOffset.x * k, raycastOffset.y, 0),Vector2.right * _direction.AsSign(),Color.red,3);
        
        if (hit.collider != null)
        {
            if (hit.distance < 2)
            {
                OnJump?.Invoke();
            }
        }
        
    }

    public float GetMovement()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > 4)
        {
            if (_player.transform.position.x < transform.position.x)
            {
                return -1;
            }
            
            return 1;
        }
        
        return 0;
    }

    private IEnumerator AttackLoop()
    {
        // Floor melee
        Debug.Log("floor melee");
                
        _animator.SetTrigger("Attack");

        StartCoroutine(Attack());
        
        yield return new WaitForSeconds(cooldown);

        StartCoroutine(AttackLoop());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackTimeOffset);
        
        float k = _direction?.AsSign() ?? 1;
        
        _attacking = true;
        
        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        Collider2D collider =
            Physics2D.OverlapBox(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0),
                attackSize, 0f, mask);
        
        if (collider != null)
        {
            if (collider.CompareTag("Player"))
            {
                _player.GetComponent<Health>().TakeDamage(1);
            }
        }

        StartCoroutine(GizmosTimingDebug());
    }

    private IEnumerator GizmosTimingDebug()
    {
        yield return new WaitForSeconds(attackTimeOffset);
        _attacking = false;
    }

    private void OnDrawGizmos()
    {
        if (_attacking)
        {
            float k = _direction?.AsSign() ?? 1;
        
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0), attackSize);
        }
    }
}
