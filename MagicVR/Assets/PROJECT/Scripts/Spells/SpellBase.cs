using UnityEngine;

public abstract class SpellBase : MonoBehaviour
{
    public string magicWords;
    public GameObject spellPrefab;

    // All these methods should be overridden because fuck interfaces
    public virtual void OnEquip()
    {
        Debug.LogError("This method should have been overridden, u fucked up");
    }

    public virtual void OnTriggerDown()
    {
        Debug.LogError("This method should have been overridden, u fucked up");
    }

    public virtual void OnTriggerHeld()
    {
        Debug.LogError("This method should have been overridden, u fucked up");
    }

    public virtual void OnTriggerUp()
    {
        Debug.LogError("This method should have been overridden, u fucked up");
    }

    // Except this one i guess
    public virtual void OnUnequip()
    {
        Destroy(this);
    }
}
