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
            collision.gameObject.GetComponent<Health>().TakeDamage(.3f);
        }
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.Translate ( _speed * Time.deltaTime * Vector2.right);
    }
}
