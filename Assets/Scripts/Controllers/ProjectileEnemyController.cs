using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileEnemyController : MonoBehaviour, IMovementController, IAttackController
{
    [SerializeField] private float cooldown;
    
    private GameObject _player;
    private Direction _direction;
    private Animator _animator;
    
    public event Action OnAttack;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _direction = GetComponent<Direction>();
        _animator = GetComponent<Animator>();

        StartCoroutine(Attack());
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
        if (Vector3.Distance(_player.transform.position, transform.position) < 5)
        {
            _animator.SetBool("Move",true);
            if (_player.transform.position.x > transform.position.x)
            {
                return -1; // if the player is to the right, move left
            }

            return 1; // otherwise move right
        }
        else
        {
            _animator.SetBool("Move",false);
            return 0;
        }
    }

    private IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(.4f);
        OnAttack?.Invoke();
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }
}
