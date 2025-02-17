using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDieController))]
public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool isInvincible = false;

    private IDieController _die;

    private void Start()
    {
        _die = GetComponent<IDieController>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        health -= damage;
        _die.OnTakeDamage();

        if (health <= 0)
        {
            _die.OnDie();
        }
    }

    public void Heal(int amount, bool overheal = false)
    {
        health += amount;
        
        if (health > maxHealth && !overheal) health = maxHealth;
    }
}
