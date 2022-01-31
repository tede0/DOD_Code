using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class HandleObstacleDamage : MonoBehaviour
{
    private CarHealthManager health;
    private WheelVehicle car;

    private void Start()
    {
        health = GetComponent<CarHealthManager>();
        car = GetComponent<WheelVehicle>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            var speedInCrash = car.Speed;
            health.TakeDamage((int)speedInCrash / 2);
        }
    }
}
