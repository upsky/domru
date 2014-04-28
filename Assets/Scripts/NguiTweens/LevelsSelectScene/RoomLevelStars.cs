using UnityEngine;
using System.Collections;

public class RoomLevelStars : MonoBehaviour
{
    [SerializeField]
    private Consts.SceneNames _level;

	void Awake () 
    {
        var score = PlayerPrefs.GetInt(_level + "_score");
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
