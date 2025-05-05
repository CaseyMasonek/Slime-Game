using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectElement : MonoBehaviour
{
    [SerializeField] private Element element;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collected element");
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SlimeController>().CollectElement(element);
            Destroy(gameObject);
        }
    }
}
