using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterBlast : MonoBehaviour
{
    [SerializeField] private float gracePeriod;
    [SerializeField] private float activeDuration;
    
    private SpriteRenderer _renderer;
    private GameObject _player;
    private Animator _animator;
    private bool _isActive;
    
    private void Start()
    {
        _player = GameObject.Find("Player");
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        _renderer.color = new Color(1, 1, 1, 0.5f);
        
        StartCoroutine(Blast());
    }

    private IEnumerator Blast()
    {
        yield return new WaitForSeconds(gracePeriod);
        _animator.SetTrigger("Blast");
        
        _renderer.color = new Color(1, 1, 1, 1);
        _isActive = true;
        
        yield return new WaitForSeconds(activeDuration);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _isActive)
        {
            _player.GetComponent<Health>().TakeDamage(1);
            _isActive = false;
        }
    }
}
