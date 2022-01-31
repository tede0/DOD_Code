using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractWithObjects : MonoBehaviour
{
    public LayerMask toIgnore;
    public float castingRadius = 2f;
    public float castingDistance = 5f;

    public GlobalHUD hud;
    public Ammo ammoToPickUp;
    public Image crosshair;

    private void RaycastCheck()
    {
        hud.ClosePanel(hud.messagePanel);
        RaycastHit sphereHit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0f));
 
        if(Physics.SphereCast(ray, castingRadius, out sphereHit, castingDistance, ~toIgnore))
        {
            if (sphereHit.transform.CompareTag("Ammo"))
            {
                ammoToPickUp = sphereHit.transform.GetComponent<Ammo>();
                var crateAnimator = sphereHit.transform.GetComponent<Animator>();
                if (!ammoToPickUp.isOpened)
                {
                    hud.OpenPanel(hud.messagePanel ,"Press F to pick up");
                }

                if(Input.GetKeyDown(KeyCode.F))
                {
                    Gun gun = GetCurrentWeapon();
                    if (CheckAmmoCompatibility(ammoToPickUp, gun))
                    {
                        var ammoToDelete = ammoToPickUp.transform.GetChild(0).gameObject;
                        DeleteAmmoInCrate(ammoToDelete, 1f);
                        crateAnimator.SetTrigger("CrateOpen");
                        gun.magazineSize += ammoToPickUp.AmountToFill;
                        ammoToPickUp.isOpened = true;
                    }
                    else
                    {
                        StartCoroutine(ShowIncorrectWeaponPanel());
                    }
                }
            }
            CheckForMedKit(sphereHit);
            CheckForGasMask(sphereHit);
        }
    }

    private void CheckForMedKit(RaycastHit hit)
    {
        if (hit.transform.CompareTag("MedKit"))
        {
            MedKit medKit = hit.transform.GetComponent<MedKit>();
            
            if (!medKit.isOpened)
                hud.OpenPanel(hud.messagePanel, "Press F to pick up");
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                var medKitAnimator = hit.transform.GetComponent<Animator>();
                PlayerStats playerToHeal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

                if (!medKit.isOpened)
                {
                    medKitAnimator.SetTrigger("OpenMedKit");
                    playerToHeal.Heal(50);
                }
            }
        }
    }

    private void CheckForGasMask(RaycastHit hit)
    {
        if (hit.transform.CompareTag("GasMask"))
        {
            hud.OpenPanel(hud.messagePanel, "Press F to pick up");
            if (Input.GetKeyDown(KeyCode.F))
            {
                var gasMask = hit.transform.gameObject;
                PlayerStats player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
                player._equipment.HasGasMask = true;
                Destroy(gasMask);
            }
        }
    }

    private void Update()
    {
        RaycastCheck();
        RemoveCrosshairOnSniper();
    }

    private Gun GetCurrentWeapon()
    {
        Gun[] guns = null;
        Gun currentGun = null;
        try
        {
            guns = GetComponentsInChildren<Gun>();
            foreach (Gun gun in guns)
            {
                if (gun.enabled == true)
                {
                    currentGun = gun;
                }
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            Debug.Log(ex);
        }

        return currentGun;
    }

    private bool CheckAmmoCompatibility(Ammo ammo, Gun gun)
    {
        if (ammo.TypeOfAmmo.ToString() == gun.name)
        {
            return true;
        }

        return false;
    }

    private void DeleteAmmoInCrate(GameObject objectToDelete, float delay)
    {
        Destroy(objectToDelete, delay);
    }

    private void RemoveCrosshairOnSniper()
    {
        if (GetCurrentWeapon().name == "Sniper")
        {
            crosshair.enabled = false;
        }
        else
        {
            crosshair.enabled = true;
        }
    }

    IEnumerator ShowIncorrectWeaponPanel()
    {
        hud.OpenPanel(hud.incorrectWeaponPanel, "");
        yield return new WaitForSeconds(1f);
        hud.ClosePanel(hud.incorrectWeaponPanel);
    }
    
}
