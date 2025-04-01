using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(IAttackController),typeof(Direction))]
public class Attack : MonoBehaviour
{
    private IAttackController _controller;
    private Direction _direction;
    
    [SerializeField] private Vector2 meleeForce;
    
    public Vector2 attackOffset;
    public Vector2 attackSize = new Vector2(1, 2);
    
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
        LayerMask layerMask = LayerMask.GetMask("Ignore Raycast");
        float k = _direction?.AsSign() ?? 1;
        Collider2D collider = Physics2D.OverlapBox(transform.position + new Vector3(attackOffset.x * k, attackOffset.y, 0), attackSize, 0f, layerMask);
        if (!collider) return;
                    
        float damage = 1;
        
        // switch (collider.GetComponent<SlimeController>().element)
        // {
        //     case Element.Air:
        //         damage = 1f;
        //         break;
        //     case Element.Water:
        //         damage = .2f;
        //         break;
        //     case Element.Earth:
        //         damage = 1.3f;
        //         break;
        //     case Element.Fire:
        //         damage = 2f;
        //         break;
        //     case Element.None:
        //         damage = 1f;
        //         break;
        // }
        // collider.GetComponent<Health>().TakeDamage(damage);
        
        collider.GetComponent<Health>().TakeDamage(damage);
        
        collider.GetComponent<Rigidbody2D>().AddForce(meleeForce * _direction.AsSign(), ForceMode2D.Impulse);
    }
}
