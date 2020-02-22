using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Spell : ScriptableObject
{
    public string magicWords;
    public GameObject[] spellPrefabs;
    public Transform origin;

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;
    public UnityEvent OnTriggerDown;
    public UnityEvent OnTriggerHeld;
    public UnityEvent OnTriggerUp;

    public void SpawnPrefab(int i)
    {
        Instantiate(spellPrefabs[i], origin);
    }
}
