using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ButtonTestGPG : MonoBehaviour
{
    [SerializeField]
    private UILabel _label;

    [SerializeField]
    private Methods _methods;

    private void Start()
    {
    }

    private void OnClick()
    {
        switch (_methods)
        {
            case Methods.Init:
                _label.text = MySocial.Init();
                break;
            case Methods.SignIn:
                _label.text = MySocial.SignIn();
                break;
            case Methods.PostingScoreToLeaderboard:
                _label.text = MySocial.PostingScoreToLeaderboard();
                break;
            case Methods.ShowingAllLeaderboardUI:
                _label.text = MySocial.ShowingAllLeaderboardUI();
                break;
            case Methods.ShowingConcreteLeaderboardUI:
                _label.text = MySocial.ShowingConcreteLeaderboardUI();
                break;
                //case Methods.Init:
                //    break;
        }
    }


    private enum Methods
    {
        Init = 1,
        SignIn = 2,
        PostingScoreToLeaderboard = 3,
        ShowingAllLeaderboardUI = 4,
        ShowingConcreteLeaderboardUI = 5,
        //UnlockingAchievement = 7,
        //IncrementingAchievement = 8,
        //ShowingAchievementsUI = 9
    }

}
