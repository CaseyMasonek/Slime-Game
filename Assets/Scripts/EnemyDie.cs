using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class EnemyDie : MonoBehaviour, IDieController
{
    public void OnDie()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        while (audio.isPlaying)
        {
            yield return null;
        }
        
        Destroy(gameObject);
    }
    
    public void OnTakeDamage() {}
}
