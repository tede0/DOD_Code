using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMinigun : MonoBehaviour
{
    [SerializeField] private Transform _rotator;
    [SerializeField] private Transform _gun;

    [SerializeField] private float rotationSpeedGun;
    [SerializeField] private float rotationSpeedRotator;
    
    private float angleRotator;
    private float angleGun;

    private void Start()
    {
        _rotator.localRotation = Quaternion.Euler(0, 0, 0);
        _gun.localRotation = Quaternion.Euler(0,0,0);
    }

    private void Update()
    {
        RotateRotator();
        RotateGun();
    }

    private void RotateRotator()
    {
        angleRotator += Input.GetAxis("Mouse X") * rotationSpeedRotator * Time.deltaTime;
        angleRotator = Mathf.Clamp(angleRotator, -80, 80);
        _rotator.localRotation = Quaternion.AngleAxis(angleRotator, Vector3.up);
    }
    
    private void RotateGun()
    {
        angleGun += Input.GetAxis("Mouse Y") * rotationSpeedGun * -Time.deltaTime;
        angleGun = Mathf.Clamp(angleGun, -40, 30);
        _gun.localRotation = Quaternion.AngleAxis(angleGun, Vector3.right);
    }
}
