using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private GameObject _player;
    private Health _health;
    
    private RectTransform _rectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _health = _player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = (float)_health.health / _health.maxHealth;
        transform.localScale = new Vector3(width, 0.1f,0);
    }
}
