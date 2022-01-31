using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    
    [SerializeField] protected bool isDead;

    public int Health => health;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        SetVarieblesOnStart();
    }

    public virtual void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            Die();
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetHealth(int amountToSet)
    {
        health = amountToSet;
        CheckHealth();
    }

    public void TakeDamage(int damageAmount)
    {
        int healthAfterHit = health - damageAmount;
        SetHealth(healthAfterHit);
    }
    
    public void Heal(int healAmount)
    {
        int healthAfterHeal = health + healAmount;
        SetHealth(healthAfterHeal);
    }

    public virtual void SetVarieblesOnStart()
    {
        maxHealth = 100;
        isDead = false;
        SetHealth(maxHealth);
    }
}
