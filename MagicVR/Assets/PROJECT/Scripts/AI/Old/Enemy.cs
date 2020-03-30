using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public int MaxHealth;
    public int Health;

    public float RagdollTime;
    public void OnHurt(int Damage)
    {
        Health -= Damage;
    }

    private void Ragdoll()
    {
        
    }

    public IEnumerator OnDeath()
    {
        
        yield return false;
    }
}
