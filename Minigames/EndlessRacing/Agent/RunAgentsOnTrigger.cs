using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAgentsOnTrigger : MonoBehaviour
{
    [SerializeField] private ZombieController[] zombieControllers;
    [SerializeField] private NavMeshAgent[] agents;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agents = GetComponentsInChildren<NavMeshAgent>();
            zombieControllers = GetComponentsInChildren<ZombieController>();
            foreach (var agent in agents)
            {
                agent.enabled = true;
            }
            
            foreach (var zombie in zombieControllers)
            {
                zombie.enabled = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Array.Clear(zombieControllers, 0, zombieControllers.Length);
            Array.Clear(agents, 0, agents.Length);
        }
    }
}
