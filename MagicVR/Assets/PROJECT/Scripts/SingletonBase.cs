using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton Preferences")]
    public bool existAcrossMultipleScenes;

    private static object sm_lock = new object();
    private static bool sm_destroying;
    private static T sm_instance;

    public static T Instance
    {
        get
        {
            if (sm_destroying)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "', you are already dead... (returning null)");
                return null;
            }

            lock (sm_lock)
            {
                if (sm_instance == null)
                {
                    sm_instance = (T)FindObjectOfType(typeof(T));

                    if (sm_instance == null)
                    {
                        var spawnedManager = new GameObject();
                        sm_instance = spawnedManager.AddComponent<T>();
                        spawnedManager.name = typeof(T).ToString() + "Manager[temp name]";
                    }
                }
                return sm_instance;
            }
        }
    }

    public virtual void Awake()
    {
        if (existAcrossMultipleScenes)
            DontDestroyOnLoad(this);
    }

    public virtual void OnApplicationQuit()
    {
        SetDestroying();
    }

    public virtual void OnDestroy()
    {
        SetDestroying();
    }

    protected void SetDestroying()
    {
        sm_destroying = true;
    }
}