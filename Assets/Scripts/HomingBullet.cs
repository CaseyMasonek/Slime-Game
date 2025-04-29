using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float homingTime = 3;

    [SerializeField] private GameObject explosion; 
    
    private GameObject _player;
    private bool _isHoming;

    private Vector3 _prevPos;
    private Vector3 _newPos;
    private Vector3 _direction;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(HomingTimer());
    }
    
    private void Update()
    {
        if (_isHoming)
        {
            // For the first few seconds, follow the player
            _prevPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, movementSpeed * Time.deltaTime);
            _newPos = transform.position;
        } else {
            // Then keep flying in the current direction
            transform.position += movementSpeed * Time.deltaTime * (_newPos - _prevPos).normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(1);
        }

        Instantiate(explosion,transform.position,Quaternion.identity);
        
        Destroy(gameObject);
    }

    private IEnumerator HomingTimer()
    {
        _isHoming = true;
        yield return new WaitForSeconds(homingTime);
        _isHoming = false;
    }
}
