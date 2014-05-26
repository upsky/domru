using UnityEngine;
using System.Collections;

public class ShowTop10 : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnClick()
    {
        MySocial.LoadScoresForLeaderboard(false, 10);


        //var sr = FindObjectOfType<ScoreResults>();
        //sr.FillTable(sr.CreateTestItems(30));
    }
}
