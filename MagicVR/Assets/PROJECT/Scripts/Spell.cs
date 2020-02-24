using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spell : MonoBehaviour
{
    public string magicWords;
    public GameObject[] spellPrefabs;
    public Transform origin;

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;
    public UnityEvent OnTriggerDown;
    public UnityEvent OnTriggerHeld;
    public UnityEvent OnTriggerUp;

    public GameObject SpawnPrefab(int i)
    {
        return Instantiate<GameObject>(spellPrefabs[i], origin);
    }

    public void Test()
    {
        Debug.Log("consume kneecaps");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
