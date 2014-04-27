using UnityEngine;
using System.Collections;

public class RecordsSceneCameraController : MonoBehaviour 
{
    void Start()
    {
        gameObject.SetActive(false);
        EventMessenger.Subscribe(GameEvent.OnFillRecordsTable, this, () => gameObject.SetActive(true));
	}
}
