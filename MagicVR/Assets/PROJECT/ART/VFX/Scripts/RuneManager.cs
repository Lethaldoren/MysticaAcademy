using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    public GameObject rune;
    public int count;
    public float radius;
    
    public List<Rune> runes = new List<Rune>();

    [ExecuteInEditMode]
    [ContextMenu("Place Runes")]
    public void PlaceRunes()
    {
        runes.ForEach(r => DestroyImmediate(r.gameObject));
        runes.Clear();

        for (int i = 0; i < count; i++)
        {
            float a = (2 * Mathf.PI) * (i / (float)count);
            Vector3 pos = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * radius;
            Quaternion rot = Quaternion.Euler(90, 0, a * Mathf.Rad2Deg - 90);
            runes.Add(new Rune(Instantiate(rune, pos, rot, transform), count));
        }
    }
    
    [ExecuteInEditMode]
    [ContextMenu("Randomize Rune Index Offsets")]
    public void RandomizeOffsets()
    {
        for (int i = 0; i < runes.Count; i++)
        {
            runes[i].RuneIndex = (int)Random.Range(0, count);
        }
    }
}

public class Rune
{
    public GameObject gameObject;
    // public Material mat;
    public float maxValue;
    private float p_runeIndex;
    [ContextMenuItem("Randomize Offset", "RandomizeOffset")]
    public float indexOffset;
    public float RuneIndex
    {
        get { return p_runeIndex; }
        set
        {
            p_runeIndex = Mathf.Repeat(value + indexOffset, maxValue);
        }
    }

    public Rune(GameObject runeObj, float maxValue)
    {
        gameObject = runeObj;
        // mat = runeObj.GetComponent<MeshRenderer>().material;
        this.maxValue = maxValue;
        indexOffset = 0;
        RuneIndex = (int)Random.Range(0, maxValue);
    }

    public void Randomize()
    {
        RuneIndex = (int)Random.Range(0, maxValue);
    }
}
