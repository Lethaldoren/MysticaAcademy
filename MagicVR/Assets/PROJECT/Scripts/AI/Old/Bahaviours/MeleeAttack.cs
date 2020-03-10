using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;
using UnityEngine.AI; 


public class MeleeAttack : StateBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    private Material material;
    private Color colour;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // remove after art implimentation
        material = GetComponent<MeshRenderer>().material;
        colour = material.color;
        material.color = Color.red;

    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        material.color = colour;
    }

    
}
