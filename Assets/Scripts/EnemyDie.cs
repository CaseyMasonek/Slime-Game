using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class EnemyDie : MonoBehaviour, IDieController
{
    public void OnDie()
    {
        Destroy(gameObject);
    }
    
    public void OnTakeDamage() {}
}
