using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Die _die;
    
    private void Start()
    {
        _die = GameObject.FindGameObjectWithTag("Player").GetComponent<Die>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _die.SetCheckpoint(transform.position);
        }
    }
}
