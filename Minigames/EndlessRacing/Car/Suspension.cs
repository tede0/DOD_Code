using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Suspension : MonoBehaviour {

    public bool removeSteeringAngle = false;
    public GameObject wheelGameObject;
    private WheelCollider _wheelCollider;
    public Vector3 localRotOffset;
    private float _lastUpdate;

    private void Start()
    {
        _lastUpdate = Time.realtimeSinceStartup;
        _wheelCollider = GetComponent<WheelCollider>();
    }
        
    private void FixedUpdate()
    {
        HandleSuspension();
    }

    private void HandleSuspension()
    {
        if (Time.realtimeSinceStartup - _lastUpdate < 1f/60f)
        {
            return;
        }
        _lastUpdate = Time.realtimeSinceStartup;

        if (wheelGameObject && _wheelCollider)
        {
            Vector3 position = new Vector3(0, 0, 0);
            Quaternion quaternion = new Quaternion();
            _wheelCollider.GetWorldPose(out position, out quaternion);

            wheelGameObject.transform.rotation = quaternion;
            if (removeSteeringAngle)
                wheelGameObject.transform.rotation = transform.parent.rotation;

            wheelGameObject.transform.localRotation *= Quaternion.Euler(localRotOffset);
            wheelGameObject.transform.position = position;

            WheelHit wheelHit;
            _wheelCollider.GetGroundHit(out wheelHit);
        }
    }
}
