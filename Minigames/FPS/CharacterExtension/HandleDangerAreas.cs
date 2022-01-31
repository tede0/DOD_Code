using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleDangerAreas : MonoBehaviour
{
    [SerializeField] private GameObject gasMaskOverlay;
    
    private PlayerStats _stats;
    private bool inArea;
    private int tickRate = 1;
    private float nextTimeToTick = 0;
    void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }
    
    void Update()
    {
        ApplyDamage();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Radioactive"))
        {
            inArea = true;
            if (PlayerHasGasMask())
                gasMaskOverlay.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Radioactive"))
        {
            inArea = false;
            if (PlayerHasGasMask())
                gasMaskOverlay.SetActive(false);
        }
    }

    private void ApplyDamage()
    {
        if (inArea && Time.time >= nextTimeToTick)
        {
            nextTimeToTick = Time.time + 1f / tickRate;
            if (!PlayerHasGasMask())
                _stats.TakeDamage(5);
        }
    }

    private bool PlayerHasGasMask()
    {
        return _stats._equipment.HasGasMask;
    }
}
