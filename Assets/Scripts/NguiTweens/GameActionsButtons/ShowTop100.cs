using UnityEngine;
using System.Collections;

public class ShowTop100 : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnClick()
    {
        MySocialFacade.LoadScoresForLeaderboard(false, 100);
    }
}
