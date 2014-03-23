using UnityEngine;

/// <summary>
/// Версия синглтона с требованием обязательного добавления объекта на сцену при обращения к нему.
/// </summary>
public abstract class RequiredMonoSingleton<T> : MonoBehaviour where T : RequiredMonoSingleton<T>
{
    private static T _instance;

    private static bool _isDestroyed;

    public static T Instance
    {
        get
        {
            //if (_instance == null)
                //_instance = new GameObject("InstanceOf" + typeof(T)).AddComponent<T>();
          
            // Problem during the creation, this should not happen
            if (_instance == null)
            {
                if (_isDestroyed)
                    Debug.LogWarning("Object with " + typeof(T) + " script is was destroyed");
                else
                    Debug.LogError("Object with " + typeof (T) + " script is not added in this scene");
            }
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
            Debug.LogError("Object with " + typeof(T) + " script could be single only", this);
        _instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        _isDestroyed = true;
    }
}
