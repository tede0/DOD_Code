using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SpeedometerUI : MonoBehaviour
{
    [SerializeField] private WheelVehicle car;
    [SerializeField] private float maxSpeed = 0.0f;
    [SerializeField] private float minArrowAngle;
    [SerializeField] private float maxArrowAngle;
    
    [SerializeField] private Text speedLabel;
    [SerializeField] private RectTransform arrow;
    
    private float speed = 0.0f;
    
    private void Update()
    {
        speed = car.Speed;

        if (speedLabel != null)
        {
            speedLabel.text = ((int) speed) + " km/h";
        }

        if (arrow != null)
        {
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minArrowAngle, maxArrowAngle, speed / maxSpeed));
        }
    }
}