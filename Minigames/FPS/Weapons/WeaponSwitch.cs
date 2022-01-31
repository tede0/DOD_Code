using System.Security.Cryptography;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    [SerializeField] private TextMeshProUGUI ammoInfoText;
    private InputAction switching;

    [SerializeField] public int damage;

    void Start()
    {
        switching = new InputAction("Scroll", binding: "<Mouse>/scroll");
        switching.AddBinding("<Gamepad>/Dpad");
        switching.Enable();
        
        SelectWeapon();
    }
    
    void Update()
    {
        Gun gun = FindObjectOfType<Gun>();
        ammoInfoText.text = gun.currentAmmo + " / " + gun.magazineSize;

        float scrollValue = switching.ReadValue<Vector2>().y;

        int previousSelected = selectedWeapon;
        
        if(scrollValue > 0)
        {
            selectedWeapon++;
            if (selectedWeapon == transform.childCount)
                selectedWeapon = 0;
        }
        else if (scrollValue < 0)
        {
            selectedWeapon--;
            if (selectedWeapon == -1)
                selectedWeapon = transform.childCount-1;
        }

        if(previousSelected != selectedWeapon)
            SelectWeapon();

    }

    private void SelectWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        transform.GetChild(selectedWeapon).gameObject.SetActive(true);
        ModifyDamage();
    }

    private void ModifyDamage()
    {
        switch (selectedWeapon)
        {
            case 0:
                damage = 5;
                break;
            case 1:
                damage = 20;
                break;
            case 2:
                damage = 100;
                break;
        }
    }
}