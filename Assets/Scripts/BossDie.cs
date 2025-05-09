using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class BossDie : MonoBehaviour, IDieController
{
    private bool _alive = true;
    
    public void OnDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void OnTakeDamage() {}
}