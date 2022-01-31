using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_AE : MonoBehaviour
{
    private void PlayHitSoundZombie()
    {
        AudioManager.instance.Play("ZombieHit");
    }
}
