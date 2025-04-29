using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDieController))]
public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public volatile bool isInvincible = false;

    private IDieController _die;

    private void Start()
    {
        _die = gameObject.GetComponent<IDieController>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Is invincible: " + isInvincible);
        
        
        if (isInvincible) return;
        
        health -= damage;
        _die.OnTakeDamage();

        if (health <= 0)
        {
            _die.OnDie();
            health = 0;
        }
    }
    
    public void Heal(float amount, bool overheal = false)
    {
        health += amount;
        
        if (health > maxHealth && !overheal) health = maxHealth;
    }
}
