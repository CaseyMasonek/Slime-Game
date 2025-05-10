using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class BossController : MonoBehaviour, IMovementController, IAttackController
{
    [SerializeField] private float cooldown;
    
    private GameObject _player;
    private Direction _direction;
    private Animator _animator;
    
    private bool _barrage;
    [SerializeField] private float barrageDuration;

    [SerializeField] private GameObject gasterBlaster;
    
    public event Action OnAttack;

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

        if (_barrage)
        {
            OnAttack?.Invoke();
        }
    }

    public float GetMovement()
    {
        // if (Vector3.Distance(_player.transform.position, transform.position) < 2)
        // {
        //     if (_player.transform.position.x > transform.position.x)
        //     {
        //         return -1; // if the player is to the right, move left
        //     }
        //
        //     return 1; // otherwise move right
        // }
        //
        // return 0;
        
        if (Vector3.Distance(transform.position, _player.transform.position) > 3)
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
        var r = new Random();
        var move = r.Next(0, 2);

        switch (move)
        {
            case 0:
                // Barrage of bullets
                
                _barrage = true;
                yield return new WaitForSeconds(barrageDuration);
                _barrage = false;
                yield return new WaitForSeconds(cooldown);
                break;
            
            case 1:
                // Gaster blaster
                _animator.SetTrigger("Attack");
                
                GameObject blaster = Instantiate(gasterBlaster, transform.position + new Vector3(5 * _direction.AsSign(),-.6f,0), Quaternion.identity);

                if (_direction.AsSign() == -1)
                {
                    Debug.Log("LEFT BLAST");
                    blaster.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                }
                
                yield return new WaitForSeconds(cooldown);
                break;
        }
        
        StartCoroutine(AttackLoop());
    }
}