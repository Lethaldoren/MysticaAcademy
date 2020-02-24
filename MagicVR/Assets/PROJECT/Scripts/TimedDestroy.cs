using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public static void DestroyAfterTime()
    {
        
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
