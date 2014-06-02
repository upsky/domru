using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class MySocialNative : MonoSingleton<MySocialNative>
{
    public static int MaxVisibleScores { get; private set; }

    private const string _leaderboardID = "com.omenra.domru.main";

    public static void SubmitScore(long score)
    {
#if UNITY_IPHONE
        Social.ReportScore(score, _leaderboardID, SubmitScoreCallback);
#endif
    }

    private static ILeaderboard _activeLeaderboard;

    public static void LoadScoresForLeaderboard(bool aroundMyRankResults, int count = 0)
    {
        MaxVisibleScores = (count > 0) ? count : int.MaxValue;
#if UNITY_IPHONE
        //PlayGameServices.loadScoresForLeaderboard(_leaderboardID, GPGLeaderboardTimeScope.AllTime, false, aroundMyRankreuslts);
        Social.LoadScores(_leaderboardID,LoadScoresCallback);

        //todo другой вариант LoadScores с указанием range
        //_activeLeaderboard.range = new Range(0, count);//http://docs.unity3d.com/ScriptReference/Social.CreateLeaderboard.html
        //Social.Active.LoadScores(_activeLeaderboard, LoadScoresWithRangeCallback); //http://docs.unity3d.com/ScriptReference/Social.Active.html, 

        Debug.LogWarning("call LoadScoresForLeaderboard");
#endif
    }

    public static void ShowLeaderboard()
    {
#if UNITY_IPHONE
        Social.ShowLeaderboardUI();
        //GameCenterPlatform.ShowLeaderboardUI(_leaderboardID,0);
#endif
    }

    private void Start()
    {
        Debug.LogWarning("<color=green>SocialNative Start</color>");
#if UNITY_IPHONE
        if (!Social.localUser.authenticated)
            Authenticate();

        //_activeLeaderboard = Social.CreateLeaderboard();

        //GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
    }

    public static void Authenticate()
    {
#if UNITY_IPHONE
        Social.localUser.Authenticate(AuthenticateCallback);
#endif
    }

    public static GPGPlayerInfo GetLocalPlayerInfo()
    {
        var playerInfo = new GPGPlayerInfo {name = Social.localUser.userName, playerId = Social.localUser.id};
        Debug.LogWarning("<color=green>GetLocalPlayerInfo=true</color>");
        return playerInfo;
    }


    private static List<GPGScore> _lastLoadedScores=new List<GPGScore>();

    private static void LoadScoresWithRangeCallback(bool success)
    {
        Debug.LogWarning("<color=green>LoadScoresWithRange=" + success + "</color>");
        LoadScoresCallback(_activeLeaderboard.scores);
    }

    private static void LoadScoresCallback(IScore[] scores)
    {
        _lastLoadedScores.Clear();
        string[] usersIds= scores.Select(s => s.userID).ToArray();
        _lastLoadedScores = scores.Select(s => new GPGScore
            {
                rank = s.rank, displayName = "", value = s.value, playerId = s.userID
            }).ToList();

        Social.LoadUsers(usersIds, LoadUsersCallback);
        Debug.LogWarning("<color=green>LoadScores=true, scoresCount=" + scores.Length + "</color>");
    }

    private static void LoadUsersCallback(IUserProfile[] users)
    {
        for (int i = 0; i < users.Count(); i++)
        {
            _lastLoadedScores[i].displayName = users[i].userName;
        }
        Debug.LogWarning("<color=green>usersCount=" + users.Length + "</color>");
        ScoresNativeManager.OnLoadScores(_lastLoadedScores);
    }


//    public void ReportAchievement(string achievementid)
//    {
//#if UNITY_IPHONE
//        Social.ReportProgress(achievementid, 100.0f, NullMe);
//#endif
//    }

//    public void ShowAchievements()
//    {
//#if UNITY_IPHONE
//        if (Social.localUser.authenticated)
//            Social.ShowAchievementsUI();
//#endif
//    }



    private static void AuthenticateCallback(bool success)
    {
        Debug.LogWarning("<color=green>Authenticate=" + success + "</color>");
    }

    private static void SubmitScoreCallback(bool success)
    {
        Debug.LogWarning("<color=green>SubmitScore=" + success + "</color>");
    }


}


public static class ScoresNativeManager
{
    // Fires when loading scores succeeds
    public static event Action<List<GPGScore>> loadScoresSucceededEvent;

    /// <summary>
    /// Применять только в классе MySocialNative
    /// </summary>
    public static void OnLoadScores(List<GPGScore> scores)
    {
        Debug.LogWarning("<color=green>OnLoadScores=true</color>");
        Debug.LogWarning("<color=green>ScoresCount=" + scores.Count + "</color>");
        loadScoresSucceededEvent.Invoke(scores);
    }
}