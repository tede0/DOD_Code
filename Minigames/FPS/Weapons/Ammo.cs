using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int AmountToFill;
    public AmmoType TypeOfAmmo;
    public bool isOpened;
    
    public enum AmmoType
    {
        Rifle,
        Pistol,
        Sniper
    }
}
