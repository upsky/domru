using UnityEngine;

public class TimeManagerForDebug : MonoBehaviour
{
    public float TimeScale = 1;

    private void Update()
    {
        Time.timeScale = TimeScale;
    }
}
