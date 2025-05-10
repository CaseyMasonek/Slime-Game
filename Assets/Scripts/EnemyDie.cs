using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class EnemyDie : MonoBehaviour, IDieController
{
    private bool _alive = true;
    
    public void OnDie()
    {
        if (_alive)
        {
            _alive = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
    
    public void OnTakeDamage() {}
}
