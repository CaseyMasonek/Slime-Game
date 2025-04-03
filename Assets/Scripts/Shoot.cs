using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(IAttackController),typeof(Direction))]
public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    
    private IAttackController _controller;
    private Direction _direction;
    
    private void Awake()
    {
        _controller = GetComponent<IAttackController>();
        _direction = GetComponent<Direction>();
    }
    
    private void OnEnable()
    {
        _controller.OnAttack += AttackAction;
    }

    private void OnDisable()
    {
        _controller.OnAttack -= AttackAction;
    }

    private void AttackAction()
    {
        Instantiate(projectilePrefab, transform.position + (Vector3.right * _direction.AsSign() * 2), Quaternion.identity);
    }
}
