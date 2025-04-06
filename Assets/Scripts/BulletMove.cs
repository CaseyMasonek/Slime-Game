using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletMove : MonoBehaviour
{
    [FormerlySerializedAs("_speed")] [SerializeField] private float speed;
    [SerializeField] private float baseDamage;
    
    
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = baseDamage;
            if (collision.gameObject.GetComponent<BasicEnemyController>() != null)
            {
                switch (collision.gameObject.GetComponent<BasicEnemyController>().element)
                {
                    case Element.None:
                        damage *= 1f;
                        break;
                    case Element.Fire:
                        damage *= 0.3f;
                        break;
                    case Element.Earth:
                        damage *= 1.2f;
                        break;
                    case Element.Air:
                        damage = 1f;
                        break;
                    case Element.Water:
                        damage = 0.1f;
                        break;
                }
            }
            collision.gameObject.GetComponent<Health>().TakeDamage(damage * (transform.localScale.x/2));
        }
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        //transform.Translate ( _speed * Time.deltaTime * Vector2.right);
        _rigidbody.velocity = speed * transform.right;
    }
}
