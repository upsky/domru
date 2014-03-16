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
            MainSceneManager.CurrentGameMode = MainSceneManager.GameMode.InvokeAdjuster;
            MainSceneManager.Door.OpenDoor();
            MainSceneManager.Adjuster.StartWalk();

            //StopCat            
            ShapesSorter.StartSorting();
        }        
    }
}
