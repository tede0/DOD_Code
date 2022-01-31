using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boss.GetComponent<Animator>().SetTrigger("Roar");
            boss.GetComponent<Boss>().enableBoss = true;
        }
    }
}
