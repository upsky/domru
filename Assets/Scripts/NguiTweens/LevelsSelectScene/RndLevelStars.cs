using UnityEngine;
using System.Collections;

public class RndLevelStars : MonoBehaviour {

	// Use this for initialization
	void Awake () 
    {
        return;
        var score=PlayerPrefs.GetInt(Consts.SceneNames.Level1 + "_score");
	    var sprite = GetComponent<UISprite>();

        switch (ScoreCounter.GetCountStars((int)score))
        {
            case 1:
                sprite.spriteName = "04_level_1_star";
                break;
            case 2:
                sprite.spriteName = "05_level_2_star";
                break;
            case 3:
                sprite.spriteName = "06_level_3_star";
                break;
        }
	}

    private void Start()
    {
    }
}
