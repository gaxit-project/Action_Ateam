using UnityEngine;
using System;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance = null)
            {
                Type type = typeof(T);

                instance = (T)FindAnyObjectByType(type);

                if(instance == null)
                {
                    Debug.LogError(type + "をアタッチしているGameObjectが存在しません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        CheckInstance();
    }

    protected void CheckInstance()
    {
        if (instance == null) { instance = this as T; return; }

        else if (Instance == this) { return; }

        Destroy(gameObject);
        return;
    }
}
