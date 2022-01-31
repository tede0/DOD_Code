using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{
    private AudioSource _audioSource;
    private WheelVehicle _car;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _car = GetComponent<WheelVehicle>();
    }
    
    private void Update()
    {
        _audioSource.pitch = _car.Speed / 100;
    }
}
