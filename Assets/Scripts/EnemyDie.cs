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
            StartCoroutine(Die());
        }
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
