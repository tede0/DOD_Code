using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdateOnEnable : MonoBehaviour {
    
    private NavMeshSurface surface;
    
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>(); 
        surface.BuildNavMesh();
    }
}