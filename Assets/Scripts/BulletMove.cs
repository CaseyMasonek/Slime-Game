using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = 0.3f;
            switch (collision.gameObject.GetComponent<BasicEnemyController>().element)
            {
                case Element.None:
                    damage = .3f;
                    break;
                case Element.Fire:
                    damage = 0;
                    break;
                case Element.Earth:
                    damage = 0.2f;
                    break;
                case Element.Air:
                    damage = 0.5f;
                    break;
                case Element.Water:
                    damage = 0.1f;
                    break;
            }
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.Translate ( _speed * Time.deltaTime * Vector2.right);
    }
}
