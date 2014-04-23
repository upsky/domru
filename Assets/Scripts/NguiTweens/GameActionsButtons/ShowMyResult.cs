using UnityEngine;
using System.Collections;

public class ShowMyResult : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnClick()
    {
        MySocial.LoadScoresForLeaderboard(true);      
    }
}
