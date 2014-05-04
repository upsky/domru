using UnityEngine;
using System.Collections;

public class RecordsSceneCameraController : MonoBehaviour 
{
    void Start()
    {
        if (!Application.isEditor)
            gameObject.SetActive(false);
        EventMessenger.Subscribe(GameEvent.OnFillRecordsTable, this, () => gameObject.SetActive(true));
	}
}
