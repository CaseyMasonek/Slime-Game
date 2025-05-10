using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterBlast : MonoBehaviour
{
    [SerializeField] private float gracePeriod;
    [SerializeField] private float activeDuration;
    
    [SerializeField] private AudioClip blastSFX;
    
    private GameObject _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GameObject _boss;
    private AudioSource _audio;
    
    private bool _isActive;
    
    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponentInChildren<Animator>();
        _boss = GameObject.Find("Boss");
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _audio = GameObject.Find("Player").GetComponent<AudioSource>();
        
        StartCoroutine(Blast());
    }

    private IEnumerator Blast()
    {
        yield return new WaitForSeconds(gracePeriod);
        _animator.SetTrigger("Blast");
        _audio.PlayOneShot(blastSFX);

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
