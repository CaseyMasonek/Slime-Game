using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private GameObject _player;
    private int _phase = 0;
    
    [SerializeField] private float explosionTime;
    [SerializeField] private float growthScale;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionLogic());
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Time.deltaTime * growthScale * Vector3.one;
    }

    private IEnumerator ExplosionLogic()
    {
        _phase = 0;

        yield return new WaitForSeconds(explosionTime);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<Health>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
