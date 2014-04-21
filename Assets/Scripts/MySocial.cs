using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class MySocial : MonoSingleton<MySocial>
{   
    #if UNITY_IPHONE || UNITY_ANDROID

    private const string LeaderboardID = "CgkIuaqBk6sCEAIQAA";

    private string text = "123";
	// Use this for initialization
	void Start ()
	{
	    Init();
        PlayGameServices.attemptSilentAuthentication();
	}

    public static void LoadScoresForLeaderboard()
    {
        PlayGameServices.loadScoresForLeaderboard(LeaderboardID, GPGLeaderboardTimeScope.AllTime, false, false);
    }

    void loadScoresSucceededEvent(List<GPGScore> scores)
    {
        text = "";
        Debug.Log("loadScoresSucceededEvent");
        Prime31.Utils.logObject(scores);

        foreach (var score in scores)
        {
            text += score.displayName + "\t";
            //text += score.formattedScore + "\t";
            text += score.rank + "\t";
            text += score.value + "\t";
            text += "\n";
        }
    }
	


    public static string Init()
    {
        PlayGameServices.enableDebugLog(true);

        // we always want to call init as soon as possible after launch. Be sure to pass your own clientId to init on iOS!
        // This call is not required on Android.
        PlayGameServices.init("160040154367.apps.googleusercontent.com", true);//только для IOS

        GPGManager.loadScoresSucceededEvent += Instance.loadScoresSucceededEvent;
        return "inited";
    }



    public static string SignIn()
    {
        string ret = "";
        // authenticate user:

        return ret;
    }

    public static string PostingScoreToLeaderboard()
    {
        string ret = "";

        return ret;
    }

    public static string ShowingConcreteLeaderboardUI()
    {
        string ret = "ShowLeaderboardUI";
        // show leaderboard UI
        PlayGameServices.showLeaderboard(LeaderboardID, GPGLeaderboardTimeScope.AllTime);
        return ret;
    }

    //public static string ShowingAllLeaderboardUI()
    //{
    //    string ret = "ShowingAllLeaderboardUI";
    //    // show leaderboard UI
    //    Social.ShowLeaderboardUI();
    //    return ret;
    //}



    //public static string LoadScores()
    //{
    //    string ret = "LoadScores";
    //    // show leaderboard UI
    //    ((PlayGamesPlatform) Social.Active).LoadScores(LeaderboardID, scores =>
    //        {
    //            if (scores.Length > 0)
    //            {
    //                Debug.Log("Got " + scores.Length + " scores");
    //                string myScores = "Leaderboard:\n";
    //                foreach (IScore score in scores)
    //                    myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
    //                ret = myScores;
    //            }
    //            else
    //                ret = "No scores loaded";
    //        });
    //    return ret;
    //}

#endif
}
