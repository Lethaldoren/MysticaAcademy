using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpellManager : SingletonBase<SpellManager>
{
    public GameObject[] inputSpells;
    public Dictionary<string, GameObject> spellDict = new Dictionary<string, GameObject>();

    public override void Awake()
    {
        base.Awake();
        foreach (GameObject newSpell in inputSpells)
        {
            spellDict.Add(newSpell.GetComponent<Spell>().magicWords, newSpell);
        }
    }

    public GameObject GetSpell(string spellName)
    {
        GameObject spell;
        // if spell is not null, return it, otherwise null
        return spellDict.TryGetValue(spellName, out spell) ? spell : null;
    }
}
