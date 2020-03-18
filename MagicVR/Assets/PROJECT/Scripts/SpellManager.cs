using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpellManager : SingletonBase<SpellManager>
{
    public Transform spellPool;

    public SpellPoolSlot[] spells;

    public string[] AllSpellNames()
    {
        return spells.ToList().Select(s => s.spellName).ToArray();
    }

    public GameObject GetSpell(string spellWords)
    {
        int i = spells.ToList().FindIndex(s => s.spellName == spellWords);

        if (i == -1)
        {
            Debug.LogError("Could not find valid spell for " + spellWords);
            return null;
        }
        else if (spells[i].taken)
        {
            Debug.Log(spellWords + " is already in use");
            return null;
        }
        else
        {
            spells[i].taken = true;
            return spells[i].obj;
        }
    }

    public void ReturnSpell(GameObject spell)
    {
        int i = spells.ToList().FindIndex(s => s.spellName == spell.GetComponent<Spell>().magicWords);
        spells[i].taken = false;
        spell.transform.SetParent(spellPool, false);
        // spell.transform.localPosition = Vector3.zero;
    }
}

[System.Serializable]
public struct SpellPoolSlot
{
    public GameObject obj;
    public string spellName;
    public bool taken;
}