using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] zombieClips;

    private AudioSource _audioSource;
    private float soundDelay = 0.2f;
    private float nextTimeToBreath = 0;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        if (Time.time >= nextTimeToBreath)
        {
            nextTimeToBreath = Time.time + 1f / soundDelay;
            _audioSource.clip = RandomSound();
            _audioSource.PlayDelayed(0f);
        }
    }

    private AudioClip RandomSound()
    {
        var randInt = Random.Range(0, zombieClips.Length);
        return zombieClips[randInt];
    }

}
