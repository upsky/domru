using UnityEngine;

public class TimeManagerForDebug : MonoBehaviour
{ 
    #if UNITY_EDITOR

    public float timeScale = 1;
    public int targetFPS = 60;

    private void Start()
    {
        Debug.LogWarning("<color=orange>QualitySettings.vSyncCount setted to 0</color>",this);
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        Time.timeScale = timeScale;
        Application.targetFrameRate = targetFPS;
    }

    #endif
}
