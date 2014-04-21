using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ScoreResults : MonoBehaviour 
{
    private Transform _grid;


	// Use this for initialization
	void Start ()
    {
        GPGManager.loadScoresSucceededEvent += FillTable;
	    //MySocial.ShowingConcreteLeaderboardUI();

	    _grid = GetComponent<UIGrid>().transform;

	    MySocial.LoadScoresForLeaderboard();

	    //FillTable(FillTableTest());
    }


    private void FillTable(List<GPGScore> scores)
    {
        Debug.Log("loadScoresSucceededEvent");
        Prime31.Utils.logObject(scores);

        int index = 0;
        foreach (var score in scores)
        {
            FillTableItem(score, _grid.GetChild(index).GetChild(0));
            index++;
        }
    }

    private void FillTableItem(GPGScore score, Transform line)
    {
        var lbl1 = line.GetChild(0).GetComponent<UILabel>();
        lbl1.text = score.rank.ToString();

        var lbl2 = line.GetChild(1).GetComponent<UILabel>();
        lbl2.text = score.displayName;

        var lbl3 = line.GetChild(2).GetComponent<UILabel>();
        lbl3.text = score.value.ToString();
    }

    private List<GPGScore> FillTableTest()
    {
        List<GPGScore> scores = new List<GPGScore>();

        var sc1 = new GPGScore();
        sc1.displayName = "Name1";
        sc1.rank = 1f;
        sc1.value = 1111;
        scores.Add(sc1);

        var sc2 = new GPGScore();
        sc2.displayName = "Name2";
        sc2.rank = 2f;
        sc2.value = 2222;
        scores.Add(sc2);
        return scores;
    }

    private void OnDestroy()
    {
        GPGManager.loadScoresSucceededEvent -= FillTable;
    }
}
