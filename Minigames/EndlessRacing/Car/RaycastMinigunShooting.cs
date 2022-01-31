using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMinigunShooting : MonoBehaviour
{
    [SerializeField] private bool _isFiring = false;
    
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem fireSmoke;
    [SerializeField] private ParticleSystem bulletShell;
    [SerializeField] private ParticleSystem[] hitEffect;

    [SerializeField] private GameObject headShotParticle;
    [SerializeField] private GameObject bloodParticle;
    
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private LayerMask toIgnore;
    
    private Ray ray;
    private RaycastHit hitInfo;

    private int fireRate = 15;
    private float accumulatedTime = 0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= accumulatedTime)
        {
            accumulatedTime = Time.time + 1f / fireRate;
            StartFiring();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        _isFiring = true;
        FireBullet();
    }
    
    private void FireBullet()
    {
        AudioManager.instance.Play("MinigunShot");
        muzzleFlash.Play();
        bulletShell.Play();
        fireSmoke.Play();

        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;
        if (Physics.Raycast(ray, out hitInfo, 1000f, ~toIgnore))
        {
            ApplyDamage(hitInfo);
            
            SpawnBulletTrail();
            foreach (var particle in hitEffect)
            {
                particle.transform.position = hitInfo.point;
                particle.transform.forward = hitInfo.normal;
                particle.Play();
            }
        }
    }
    
    private void StopFiring()
    {
        _isFiring = false;
    }

    private void SpawnBulletTrail()
    {
        var bulletTrailEffect = Instantiate(bulletTrail, ray.origin, Quaternion.identity);
        bulletTrailEffect.AddPosition(ray.origin);
        bulletTrailEffect.transform.position = hitInfo.point;
    }

    private void ApplyDamage(RaycastHit hit)
    {
        if (hit.transform.CompareTag($"Zombie"))
        {
            EnemyStats hitStats = hit.transform.GetComponentInParent<EnemyStats>();
            HitBoxHead headshot = hit.transform.GetComponent<HitBoxHead>();
            if (headshot)
            {
                AudioManager.instance.Play("Headshot");
                SpawnParticle(headShotParticle, hit, false);
                hitStats.Die();
            }
            else
            {
                hitStats.TakeDamage(15);
                SpawnParticle(bloodParticle, hit, true);
            }
        }
    }
    
    private void SpawnParticle(GameObject particle, RaycastHit hit, bool transformParent)
    {
        Quaternion rotation = Quaternion.LookRotation(hit.normal);
        GameObject impactDestroy = Instantiate(particle, hit.point, rotation);
        
        if (transformParent)
            impactDestroy.transform.parent = hit.transform;
        
        Destroy(impactDestroy, 3);
    }
}
