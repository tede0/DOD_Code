using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    private PlayerHUD _hud;
    public Equipment _equipment;

    private void Start()
    {
        _hud = GetComponent<PlayerHUD>();
        _equipment = GetComponent<Equipment>();
        SetVarieblesOnStart();
    }

    private void Update()
    {
        SetScreenOnDeath();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        _hud.UpdateHealth(health, maxHealth);
    }

    private void SetScreenOnDeath()
    {
        if (isDead)
        {
            SceneManager.LoadScene("IsDead");
        }
    }
}
