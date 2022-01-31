using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private Transform _target;
    
    private NavMeshAgent _agent = null;
    private EnemyStats _enemyStats = null;
    private Animator _animator;

    private float timeFromLastAttack = 0;
    private bool applyRootMotion;
    
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        applyRootMotion = _animator.applyRootMotion;
        _enemyStats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        applyRootMotion = true;
        _agent.SetDestination(_target.position);
        _animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
        
        if (!_enemyStats.IsDead())
            RotateToTarget();

        float distance = GetCurrentDistance();
        if (distance <= _agent.stoppingDistance)
        {
            applyRootMotion = false;
            _animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);
            //attack
            if ((Time.time >= timeFromLastAttack + _enemyStats.attackSpeed) && !_enemyStats.IsDead())
            {
                timeFromLastAttack = Time.time;
                CharacterStats targetToAttack = _target.GetComponent<CharacterStats>();
                StartCoroutine(WaitForAnimAttack(targetToAttack));
            }
        }
    }

    private void AttackTarget(CharacterStats targetToDamage)
    {
        if (GetCurrentDistance() <= 3)
            _enemyStats.DealDamage(targetToDamage);
    }

    private void RotateToTarget()
    {
        Vector3 direction = _target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    private float GetCurrentDistance()
    {
        float distance = Vector3.Distance(transform.position, _target.position);
        return distance;
    }

    public void StopAgentAfterDeath()
    {
        _agent.speed = 0;
        _agent.isStopped = true;
        _agent.destination = transform.position;
    }

    IEnumerator WaitForAnimAttack(CharacterStats targetToDamage)
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        AttackTarget(targetToDamage);
    }
    
}
