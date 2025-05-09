using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class MinibossController : MonoBehaviour, IMovementController
{
    [SerializeField] private float cooldown;
    
    private GameObject _player;
    private Direction _direction;
    private Animator _animator;

    private Vector2 attackSize = new Vector2(3, 1);
    private Vector2 attackOffset = new Vector2(2, -2);
    
    private int _movement;

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
        yield return new WaitForSeconds(.5f);
        
        float k = _direction?.AsSign() ?? 1;
        
        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        Collider2D collider =
            Physics2D.OverlapBox(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0),
                attackSize, 0f, mask);
        
        if (collider != null)
        {
            Debug.Log(collider.name);
            
            if (collider.CompareTag("Player"))
            {
                Debug.Log("what");
                _player.GetComponent<Health>().TakeDamage(1);
            }
        }
        
        _animator.SetTrigger("Attack");
    }

    private void OnDrawGizmos()
    {
        float k = _direction?.AsSign() ?? 1;
        
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0), attackSize);
    }
}