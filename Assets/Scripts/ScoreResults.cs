using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ScoreResults : MonoBehaviour
{
    [SerializeField]
    private GameObject _linePrefab;

    private Transform _grid;

    #if UNITY_IPHONE || UNITY_ANDROID
	// Use this for initialization
	void Start ()
    {
	    _grid = GetComponent<UIGrid>().transform;

        GPGManager.loadScoresSucceededEvent += FillTable;

        //MySocial.ShowingConcreteLeaderboardUI();

	    //FillTable(FillTableTest(3));

        //todo после 28-го протестить на >1000 игроков. Хотя не надо, т.к. всего 25 за раз присылается и больше 100 не отобразиться.
    }


    private void FillTable(List<GPGScore> scores)
    {
        Debug.Log("loadScoresSucceededEvent");
        Prime31.Utils.logObject(scores);

        foreach (Transform tr in _grid)
            Destroy(tr);

        countScores = scores.Count;

        int index = 0;
        //int yPos = 0;
        foreach (var score in scores)
        {
            var lineItemGO = NGUITools.AddChild(_grid.gameObject, _linePrefab);
            //var lineItemUI = lineItemGO.GetComponent<UIWidget>();

            index++;

            if (index < 10)
            lineItemGO.name = "00"+index.ToString();
            else if (index > 9 && index < 100)
                lineItemGO.name = "0" + index.ToString();
            else if (index > 99)// && yPos < 1000)
                lineItemGO.name = index.ToString();

            AddGridLine(score, lineItemGO.transform.GetChild(0));
            
        }
        _grid.GetComponent<UIGrid>().Reposition();
    }

    private int countScores = 0;

    private void AddGridLine(GPGScore score, Transform line)
    {
        var lbl1 = line.GetChild(0).GetComponent<UILabel>();
        lbl1.text = score.rank.ToString();

        var lbl2 = line.GetChild(1).GetComponent<UILabel>();
        lbl2.text = countScores.ToString();//score.displayName;

        var lbl3 = line.GetChild(2).GetComponent<UILabel>();
        lbl3.text = score.value.ToString();


        var sprite = line.GetSafeComponent<UISprite>();
        if (MySocial.GetLocalPlayerInfo()!=null && score.playerId == MySocial.GetLocalPlayerInfo().playerId)
        {
            sprite.spriteName = "06_line_red";
        }
        else
        {
             sprite.spriteName = "05_line";
        }

    }

    private List<GPGScore> FillTableTest(int n)
    {
        List<GPGScore> scores = new List<GPGScore>();

        for (int i = 0; i < n; i++)
        {
            var sc = new GPGScore();
            sc.displayName = "Name"+i;
            sc.rank = i;
            sc.value = i+i;
            scores.Add(sc);
        }
        return scores;
    }

    private void OnDestroy()
    {
        GPGManager.loadScoresSucceededEvent -= FillTable;
    }
#endif
}
