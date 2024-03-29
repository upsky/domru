using UnityEngine;

/// <summary>
/// ������ ��������� � ����������� ������������� ���������� ������� �� ����� ��� ��������� � ����. ������������ ��� ��������� ������ ��� ���������� ����.
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
                {
#if UNITY_EDITOR
                    Debug.LogWarning("Object with " + typeof (T) + " script is was destroyed");
#endif
                }
                else
                    Debug.LogError("Object with " + typeof (T) + " script is not added in this scene");
            }
            return _instance;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isDestroyed = true;
        _instance = null;
    }
    protected virtual void OnDestroy()
    {
        _isDestroyed = true;
        _instance = null;
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Object with " + typeof (T) + " script could be single only", this);
            Destroy(gameObject);
            return;
        }
        _isDestroyed = false;//������� �����, �.�. Awake ����������� ������ ��� �������� �����, ���� ������ �������� ������������� � �����.
        _instance = this as T;
    }
}
