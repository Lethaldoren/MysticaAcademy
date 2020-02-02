using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    public GameObject runeTemplate;
    public int count;
    public float radius;

    public Sprite[] runes;

    [ExecuteInEditMode]
    [ContextMenu("Spawn Runes")]
    public void SpawnRunes()
    {
        for (int i = 0; i < count; i++)
        {
            float a = (2 * Mathf.PI) * (i / (float)count);
            Vector3 pos = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * radius;
            Quaternion rot = Quaternion.Euler(90, 0, a * Mathf.Rad2Deg - 90);
            Instantiate(runeTemplate, pos, rot, transform);
        }
    }
    
    [ExecuteInEditMode]
    [ContextMenu("Randomize Rune Index Offsets")]
    public void RandomizeTextures()
    {
        var r = transform.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < r.Length; i++)
        {
            r[i].sprite = runes[Random.Range(0, runes.Length)];
        }
    }
}