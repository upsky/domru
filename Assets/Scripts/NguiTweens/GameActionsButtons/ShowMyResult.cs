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

        //test only
        var sr = FindObjectOfType<ScoreResults>();
        sr.FillTable(sr.CreateTestItems(30));
    }
}
