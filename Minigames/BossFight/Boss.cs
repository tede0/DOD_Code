using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject echoSlam;
    
    private Animator _animator;
    private Transform _target;
    private EnemyStats _stats;
    private bool isAngry;
    private int health;

    private float jumpRate = 0.1f;
    private float nextTimeToJump = 0;
    
    public bool enableBoss;
    private float currentDistance;

    private Vector3 echoSlamPos = new Vector3(0, 0.2f, 5);
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _stats = GetComponent<EnemyStats>();
        _target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }

    private void Update()
    {
        health = _stats.Health;
        currentDistance = (_target.position - transform.position).magnitude;

        CheckAngry();
        BossJumpOnLowHp();

        if (enableBoss && !_stats.IsDead())
        {
            RotateBoss();
            MoveBoss();
        }
    }

    private void RotateBoss()
    {
        Vector3 direction = _target.position - transform.position;
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(direction), 5 * Time.deltaTime);
    }

    private void MoveBoss()
    {
        if (currentDistance > 3)
        {
            if (isAngry)
                _animator.SetFloat("Speed", 2);
            else
                _animator.SetFloat("Speed", 1f);
        }

        if (currentDistance <= 2)
        {
            BossAttack();
        }
    }

    private void BossAttack()
    {
        _animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);
        _animator.SetTrigger("Attack" + RandomizeAttack());
    }

    private void BossJumpOnLowHp()
    {
        if (health <= _stats.MaxHealth / 4)
        {
            if (Time.time >= nextTimeToJump)
            {
                _animator.SetTrigger("Jump");
                nextTimeToJump = Time.time + 1f / jumpRate;
            }
        }
    }

    private void PlayEchoSlam()
    {
        Instantiate(echoSlam, transform.position + echoSlamPos, Quaternion.identity);
        var isTargetGroundedAtSlam = _target.GetComponent<FirstPersonController>().Grounded;
        if (isTargetGroundedAtSlam && currentDistance <= 20f)
        {
            _target.GetComponent<PlayerStats>().TakeDamage(25);
        }
    }

    private void HitPlayerIfNear()
    {
        if (currentDistance <= 4f && _animator.GetCurrentAnimatorStateInfo(0).IsName("BossAttack2"))
        {
            _target.GetComponent<PlayerStats>().TakeDamage(15);
        }
        else if (currentDistance <= 3f)
        {
            _target.GetComponent<PlayerStats>().TakeDamage(10);
        }
        else
        {
            return;
        }
    }

    private void CheckAngry()
    {
        if (health <= _stats.MaxHealth / 2 && !isAngry)
        {
            isAngry = true;
        }
    }

    private void PlayRoar()
    {
        AudioManager.instance.Play("BossRoar");
    }
    

    private int RandomizeAttack()
    {
        return Random.Range(1, 3);
    }
    
    private void SetBossState(bool state)
    {
        enableBoss = state;
    }
}
