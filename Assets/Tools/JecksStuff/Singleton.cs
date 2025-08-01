using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component 
{
    protected static T instance;

    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance 
    {
        get 
        {
            if (instance == null) 
            {
                instance = FindAnyObjectByType<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake() 
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton() 
    {
        if (!Application.isPlaying) return;

        instance = this as T;
    }
}