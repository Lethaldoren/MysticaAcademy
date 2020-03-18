using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Spell : MonoBehaviour
{
    public string magicWords;

    public UnityEvent OnEquip;
    public UnityEvent OnUnequip;
    public UnityEvent OnTriggerDown;
    public UnityEvent OnTriggerHeld;
    public UnityEvent OnTriggerUp;

    public void Test()
    {
        Debug.Log("consume kneecaps");
    }
}
