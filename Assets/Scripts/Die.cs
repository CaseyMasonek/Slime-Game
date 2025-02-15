using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Die : MonoBehaviour
{
    private Health _health;
    
    private void Start()
    {
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (_health.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
