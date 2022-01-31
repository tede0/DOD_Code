using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class DriveBy : MonoBehaviour
{
    private WheelVehicle car;
    private EnemyStats _enemyStats;

    private void Start()
    {
        car = GetComponent<WheelVehicle>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            _enemyStats = other.gameObject.GetComponentInParent<EnemyStats>();
            var currentSpeed = car.Speed;

            if (currentSpeed >= 20f)
            {
                _enemyStats.Die();
            }
        }
    }
}
