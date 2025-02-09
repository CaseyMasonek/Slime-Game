using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die))]
public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;

    private Die _die;

    private void Start()
    {
        _die = GetComponent<Die>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0 )
        {
            Debug.Log("Dead");
        }
    }

    public void Heal(int amount, bool overheal = false)
    {
        health += amount;
        
        if (health > maxHealth && !overheal) health = maxHealth;
    }
}
