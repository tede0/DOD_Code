using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int _baseValue;
    private int _maxValue;
    private int maxAlpha = 255;
    private int maxFill = 100;
    [SerializeField] private bool showBloodOverlay;

    [SerializeField] private Image fill;
    [SerializeField] private Text amount;
    
    [SerializeField] private Image bloodOverlay;
    
    public void SetValues(int baseValue, int maxValue)
    {
        _baseValue = baseValue;
        _maxValue = maxValue;
        amount.text = _baseValue.ToString();
        CalculateFillAmount();
    }
    
    private void CalculateFillAmount()
    {
        float fillAmount = (float)_baseValue / (float)_maxValue;
        fill.fillAmount = fillAmount;
    }

    private void Update()
    {
        if (showBloodOverlay)
            CalculateBloodOverlay();
    }

    private void CalculateBloodOverlay()
    {
        Color bloodToSet = bloodOverlay.color;
        bloodToSet.a = 1 - fill.fillAmount;
        bloodOverlay.color = bloodToSet;
    }
}
