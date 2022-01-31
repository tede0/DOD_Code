using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairOnHit : MonoBehaviour
{
    [SerializeField] private Image crosshair;

    public void CrosshairColorBlink()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        crosshair.color = Color.red;
        yield return new WaitForSeconds(0.09f);
        crosshair.color = Color.white;
    }
}
