using UnityEngine;
using System.Collections;

public class UIPanelFix : MonoBehaviour
{
    private UIPanel panel;

    private void Awake()
    {
        panel = GetComponent<UIPanel>();
        panel.alpha = 0f;
    }

    private void Start()
    {
        EventMessenger.Subscribe(GameEvent.StartGameProcess, this, () => panel.alpha=1f);
    }

	
}
