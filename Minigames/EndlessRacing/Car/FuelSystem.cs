using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour {
    
    [SerializeField] private float maximumFuelAmount = 100f;
    [SerializeField] private Slider fuelIndication;
    [SerializeField] private Text fuelIndicationText;
    
    public float fuelConsumptionOverTime;
    public float fuelAtTheStart;

    private void Start ()
    {
        SetVarieblesOnStart();
    }
	

    public void ReduceFuel()
    {
        fuelAtTheStart -= Time.deltaTime * fuelConsumptionOverTime;
        UpdateUI();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("GasStation"))
        {
            var amountToAddWithTime = Time.deltaTime * 5f;
            fuelAtTheStart += amountToAddWithTime;

            if(fuelAtTheStart > maximumFuelAmount)
            {
                fuelAtTheStart = maximumFuelAmount;
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        fuelIndication.value = fuelAtTheStart;
        fuelIndicationText.text = "Fuel left: " + fuelAtTheStart.ToString("0") + "%";

        if(fuelAtTheStart <=0)
        {
            fuelAtTheStart = 0;
            fuelIndicationText.text = "Out of fuel!!!";
        }
    }

    private void SetVarieblesOnStart()
    {
        if(fuelAtTheStart > maximumFuelAmount)
        {
            fuelAtTheStart = maximumFuelAmount;
        }
        fuelIndication.maxValue = maximumFuelAmount;
        UpdateUI();
    }
}