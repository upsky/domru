using UnityEngine;
using System.Collections;

public class InvokeAdjusterAction : MonoBehaviour, IGameAction
{
    private void Start()
    {
        enabled = false;
    }

    public void Execute()
    {
        if (MainSceneManager.CurrentGameMode == MainSceneManager.GameMode.Normal)
        {
            EventMessenger.SendMessage(GameEvent.InvokeAdjuster, this);
            MainSceneManager.CurrentGameMode = MainSceneManager.GameMode.InvokeAdjuster;
        }        
    }
}
