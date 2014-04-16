using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MySocial : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Init();
	}

    public static string Init()
    {
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        return "Inited";
    }
	

    public static string SignIn()
    {
        string ret = "";
        // authenticate user:
        Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                    ret= "SignIn true";
                else
                    ret = "SignIn FALSE";
                // handle success or failure
            });
        return ret;
    }

    //public static string UnlockingAchievement()
    //{
    //    string ret = "";
    //    // unlock achievement (achievement ID "Cfjewijawiu_QA")
    //    Social.ReportProgress("Cfjewijawiu_QA", 100.0f, (bool success) =>
    //        {
    //            // handle success or failure
    //            if (success)
    //                ret = "UnlockingAchievement true";
    //            else
    //                ret ="UnlockingAchievement FALSE";
    //        });
    //    return ret;
    //}

    //public static string IncrementingAchievement()
    //{
    //    string ret = "";
    //    // increment achievement (achievement ID "Cfjewijawiu_QA") by 5 steps
    //    ((PlayGamesPlatform)Social.Active).IncrementAchievement(
    //        "Cfjewijawiu_QA", 5, (bool success) =>
    //        {
    //            // handle success or failure
    //            if (success)
    //                ret ="IncrementingAchievement true";
    //            else
    //                ret = "IncrementingAchievement FALSE";
    //        });
    //    return ret;
    //}

    public static string PostingScoreToLeaderboard()
    {
        string ret = "";
        // post score 12345 to leaderboard ID "Cfji293fjsie_QA")
        Social.ReportScore(999, "CgkIuaqBk6sCEAIQAA", (bool success) =>
            {
                // handle success or failure
                if (success)
                    ret = "PostingScoreToLeaderboard true";
                else
                    ret = "PostingScoreToLeaderboard FALSE";
            });
        return ret;
    }


    //public static string ShowingAchievementsUI()
    //{
    //    string ret = "ShowingAchievementsUI";
    //    // show achievements UI //This will show a standard UI appropriate for the look and feel of the platform (Android or iOS).
    //    Social.ShowAchievementsUI();
    //    return ret;
    //}

    public static string ShowingAllLeaderboardUI()
    {
        string ret = "ShowingAllLeaderboardUI";
        // show leaderboard UI
        Social.ShowLeaderboardUI();
        return ret;
    }

    public static string ShowingConcreteLeaderboardUI()
    {
        string ret = "ShowLeaderboardUI";
        // show leaderboard UI
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIuaqBk6sCEAIQAA");
        return ret;
    }

}
