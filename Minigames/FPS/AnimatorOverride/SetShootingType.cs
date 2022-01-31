using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetShootingType : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController[] animatorOverrideControllers;
    [SerializeField] private AnimatorOverrider animatorOverrider;

    public void Set(int value)
    {
        animatorOverrider.SetAnimations(animatorOverrideControllers[value]);
    }
}
