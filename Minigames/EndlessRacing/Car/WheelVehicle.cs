using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelVehicle : MonoBehaviour
{
    [SerializeField] private bool isPlayer = true;
    [SerializeField] private string throttleInput = "Throttle";
    [SerializeField] private string brakeInput = "Brake";
    [SerializeField] private string turnInput = "Horizontal";
    [SerializeField] private string driftInput = "Drift";

    [SerializeField] private AnimationCurve turnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Quaternion spawnRotation;

    [SerializeField] private WheelCollider[] driveWheel;
    [SerializeField] private WheelCollider[] turnWheel;

    private bool isGrounded = false;
    private int lastGroundCheck = 0;

    public bool IsGrounded
    {
        get
        {
            if (lastGroundCheck == Time.frameCount)
                return isGrounded;

            lastGroundCheck = Time.frameCount;
            isGrounded = true;
            foreach (WheelCollider wheel in _wheels)
            {
                if (!wheel.gameObject.activeSelf || !wheel.isGrounded)
                    isGrounded = false;
            }

            return isGrounded;
        }
    }

    [SerializeField] AnimationCurve motorTorque = new AnimationCurve(new Keyframe(0, 200), new Keyframe(50, 300), new Keyframe(200, 0));
    [SerializeField] private float diffGearing = 4.0f;
    [SerializeField] private float brakeForce = 1500.0f;
    [SerializeField] private float steerAngle = 30.0f;
    [SerializeField] private float steerSpeed = 0.2f;
    [SerializeField] private float driftIntensity = 1f;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float downforce = 1.0f;

    private float steering;
    private float throttle;

    [SerializeField] private bool handbrake;

    [HideInInspector] public bool allowDrift = true;
    private bool drift;
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private ParticleSystem[] gasParticles;

    public float Speed => speed;

    private Rigidbody _rigidbody;
    private WheelCollider[] _wheels;
    private FuelSystem _fuelSystem;

    private void Start()
    {
        SetVarieblesOnStart();
    }
    
    private void Update()
    {
        foreach (ParticleSystem gasParticle in gasParticles)
        {
            gasParticle.Play();
            ParticleSystem.EmissionModule particleEmission = gasParticle.emission;
            particleEmission.rateOverTime = handbrake ? 0 : Mathf.Lerp(particleEmission.rateOverTime.constant, Mathf.Clamp(150.0f * throttle, 30.0f, 100.0f), 0.1f);
        }
    }
    
    private void FixedUpdate()
    {
        if (_fuelSystem.fuelAtTheStart <= 0)
        {
            foreach (WheelCollider wheel in _wheels)
            {
                wheel.brakeTorque = Mathf.Abs(throttle) * brakeForce;
            }
            return;
        }
        
        speed = transform.InverseTransformDirection(_rigidbody.velocity).z * 3.6f;
        _fuelSystem.fuelConsumptionOverTime = Mathf.Abs(speed) * 0.01f;
        _fuelSystem.ReduceFuel();
        
        if (isPlayer)
        {
            if (throttleInput != "" && throttleInput != null)
            {
                throttle = GetInput(throttleInput) - GetInput(brakeInput);
            }
            
            steering = turnInputCurve.Evaluate(GetInput(turnInput)) * steerAngle;
            drift = GetInput(driftInput) > 0 && _rigidbody.velocity.sqrMagnitude > 100;
        }
        
        foreach (WheelCollider wheel in turnWheel)
        {
            wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steering, steerSpeed);
        }

        foreach (WheelCollider wheel in _wheels)
        {
            wheel.brakeTorque = 0;
        }
        
        if (Mathf.Abs(speed) < 4 || Mathf.Sign(speed) == Mathf.Sign(throttle))
        {
            foreach (WheelCollider wheel in driveWheel)
            {
                wheel.motorTorque = throttle * motorTorque.Evaluate(speed) * diffGearing / driveWheel.Length;
            }
        }
        else
        {
            foreach (WheelCollider wheel in _wheels)
            {
                wheel.brakeTorque = Mathf.Abs(throttle) * brakeForce;
            }
        }
        
        if (drift && allowDrift)
        {
            var driftingForceApplied = -transform.right;
            driftingForceApplied.y = 0.0f;
            driftingForceApplied.Normalize();

            if (steering != 0)
                driftingForceApplied *= _rigidbody.mass * speed / 7f * throttle * steering / steerAngle;
            
            var driftTorque = transform.up * 0.1f * steering / steerAngle;


            _rigidbody.AddForce(driftingForceApplied * driftIntensity, ForceMode.Force);
            _rigidbody.AddTorque(driftTorque * driftIntensity, ForceMode.VelocityChange);
        }
        _rigidbody.AddForce(-transform.up * speed * downforce);
    }
    
    private float GetInput(string input)
    {
        return Input.GetAxis(input);
    }

    private void SetVarieblesOnStart()
    {
        _fuelSystem = GetComponent<FuelSystem>();
        _rigidbody = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (_rigidbody != null && centerOfMass != null)
        {
            _rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        _wheels = GetComponentsInChildren<WheelCollider>();
        
        foreach (WheelCollider wheel in _wheels)
        {
            wheel.motorTorque = 0.0001f;
        }
    }
}