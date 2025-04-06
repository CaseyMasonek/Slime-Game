using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private float yScale = .1f;
    private Health _health;

    private float _originalX;
    private RectTransform _rectTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (entity == null) entity = GameObject.FindGameObjectWithTag("Player");
        _health = entity.GetComponent<Health>();
        _originalX = transform.localScale.x;
    }

    // Update is called once per frame
    private void Update()
    {
        float width = _health.health / _health.maxHealth;
        transform.localScale = new Vector3(_originalX * width,yScale,0);
    }
}
