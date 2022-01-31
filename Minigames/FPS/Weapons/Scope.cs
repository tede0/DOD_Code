using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    [SerializeField] private GameObject scopeOverlay;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Camera weaponRenderCam;
    [SerializeField] private CinemachineVirtualCamera cvc;

    private Animator _animator;
    private InputAction _scope;

    private bool _isScoped;
    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        
        _scope = new InputAction("Scope", binding: "<mouse>/rightButton");
        _scope.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Gun gun = FindObjectOfType<Gun>();
        if (gun.isReloading || gun.currentAmmo == 0)
        {
            OnUnscoped();
        }
        else
        {
            if (_scope.IsPressed())
            {
                _isScoped = true;
                StartCoroutine(OnScoped());
            }
            else
            {
                OnUnscoped();
            }
        }
    }

    private void OnUnscoped()
    {
        _isScoped = false;
        scopeOverlay.SetActive(false);
        _animator.SetBool("isScoped", false);
        cvc.m_Lens.FieldOfView = 40;
        fpsCam.cullingMask = fpsCam.cullingMask | (1 << 9);
        weaponRenderCam.cullingMask = weaponRenderCam.cullingMask | (1 << 9);
    }

    IEnumerator OnScoped()
    {
        _animator.SetBool("isScoped", true);
        yield return new WaitForSeconds(0.25f);
        cvc.m_Lens.FieldOfView = 10;
        fpsCam.cullingMask = fpsCam.cullingMask & ~(1 << 9);
        weaponRenderCam.cullingMask = weaponRenderCam.cullingMask & ~(1 << 9);
        scopeOverlay.SetActive(true);
    }
}
