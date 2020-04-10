using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public static void DestroyAfterTime(GameObject obj, float delay)
    {
        obj.AddComponent<TimedDestroy>().delay = delay;
    }

    public float delay;
    float timeRemaining;

    void Start()
    {
        timeRemaining = delay;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }
}
