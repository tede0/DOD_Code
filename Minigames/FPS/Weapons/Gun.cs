using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private int fireRate = 10;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private int maxAmmo;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform fpsCam;
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private GameObject headshotParticle;
    [SerializeField] private CrosshairOnHit _crosshairOnHit;

    private InputAction shoot;
    private WeaponSwitch weaponSwitch;
    private SetShootingType shootingType;
    
    private bool isShooting;
    public bool isReloading;
    private float nextTimeToFire = 0;
    
    public int currentAmmo;
    public int magazineSize;
    public string name;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        weaponSwitch = GetComponentInParent<WeaponSwitch>();
        shootingType = GetComponentInParent<SetShootingType>();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shoot.AddBinding("<Gamepad>/x");
        
        shoot.Enable();
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }

    private void Update()
    {
        AddAmmo();
        if (currentAmmo == 0 && magazineSize == 0)
        {
            animator.SetBool("isShooting", false);
            return;
        }

        if (isReloading)
        {
            return;
        }
        
        isShooting = shoot.ReadValue<float>() == 1;
        animator.SetBool("isShooting", isShooting);

        if (isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
        GunCheckForReload();
    }

    public void Fire()
    {
        HandleShootingAudio();
        HandleAnimator();
        
        RaycastHit hit;
        muzzleFlash.Play();
        currentAmmo--;
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if (hit.transform.CompareTag($"Zombie"))
            {
                EnemyStats hitStats = hit.transform.GetComponentInParent<EnemyStats>();
                HitBoxHead headshot = hit.transform.GetComponent<HitBoxHead>();
                var isBoss = hitStats.IsBoss;
                if (headshot)
                {
                    AudioManager.instance.Play("Headshot");
                    SpawnParticle(headshotParticle, hit, false);
                    if (isBoss)
                        hitStats.TakeDamage(weaponSwitch.damage * 5);
                    else
                        hitStats.Die();
                }
                else
                {
                    hitStats.TakeDamage(weaponSwitch.damage);
                    SpawnParticle(bloodParticle, hit, true);
                }
                _crosshairOnHit.CrosshairColorBlink();
            }
            else
            {
                SpawnParticle(impactEffect, hit, true);
            }
        }
    }
    
    private void GunCheckForReload()
    {
        if (currentAmmo == 0 && magazineSize > 0 && !isReloading)
        {
            StartCoroutine(Reload());
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

    private void HandleAnimator()
    {
        switch (weaponSwitch.selectedWeapon)
        {
            case 0:
                shootingType.Set(0);
                break;
            case 1:
                shootingType.Set(1);
                break;
            case 2:
                shootingType.Set(2);
                break;
            default:
                animator.enabled = false;
                break;
        }
    }

    private void HandleShootingAudio()
    {
        switch (weaponSwitch.selectedWeapon)
        {
            case 0:
                AudioManager.instance.Play("ShootRifle");
                break;
            case 1:
                AudioManager.instance.Play("ShootPistol");
                break;
            case 2:
                AudioManager.instance.Play("ShootSniper");
                break;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("isReloading", false);
        if (magazineSize >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            magazineSize -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineSize;
            magazineSize = 0;
        }
        isReloading = false;
    }

    private void AddAmmo()
    {
        //magazineSize
    }

}