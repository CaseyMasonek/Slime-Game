using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Die _die;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isActive;
    
    private void Start()
    {
        _die = GameObject.FindGameObjectWithTag("Player").GetComponent<Die>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isActive)
        {
            _die.SetCheckpoint(transform.position);
            _animator.enabled = true;
            _isActive = true;
            _audioSource.Play();
        }
    }
}
