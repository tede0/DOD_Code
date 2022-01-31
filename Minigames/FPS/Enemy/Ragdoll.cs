using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Animator _animator;
    
    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        DeactivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = false;
        }

        _animator.enabled = false;
    }

    public void DeactivateRagdoll()
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }

        _animator.enabled = true;
    }
    
}
