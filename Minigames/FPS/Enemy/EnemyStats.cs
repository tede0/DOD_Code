using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int damage;
    public float attackSpeed;

    [SerializeField] private bool canAttack;
    [SerializeField] private bool isBoss;

    private Ragdoll _ragdoll;
    private ZombieController _zombieController;
    private ZombieAudio _zombieAudio;

    public bool IsBoss => isBoss;
    
    private void Start()
    {
        SetVarieblesOnStart();
    }
    
    public void DealDamage(CharacterStats targetToDamage)
    {
        targetToDamage.TakeDamage(damage);
    }
    
    public override void Die()
    {
        base.Die();
        _ragdoll.ActivateRagdoll();
        _zombieController.StopAgentAfterDeath();
        _zombieAudio.enabled = false;
        Destroy(gameObject, 10);
    }

    public override void SetVarieblesOnStart()
    {
        _zombieAudio = GetComponent<ZombieAudio>();
        _ragdoll = GetComponent<Ragdoll>();
        _zombieController = GetComponent<ZombieController>();

        SetHealth(maxHealth);
        isDead = false;
        
        canAttack = true;
    }
}
