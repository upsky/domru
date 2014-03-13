using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject("InstanceOf" + typeof(T)).AddComponent<T>();
          
            // Problem during the creation, this should not happen
            if (_instance == null)
                Debug.LogError("Problem during the creation of " + typeof(T));

            return _instance;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _instance = null;
    }

    protected virtual void Awake()
    {
        if (_instance != null)
            Debug.LogError("Object with " + _instance.GetType() + " script could be single only", this);
        _instance = this as T;
    }
}
