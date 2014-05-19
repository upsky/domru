using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class MySocial : MonoSingleton<MySocial>
{
    public static void SubmitScore(long score)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        PlayGameServices.submitScore(LeaderboardID, score);
#endif
    }

    public static void LoadScoresForLeaderboard(bool aroundMyRankreuslts)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        PlayGameServices.loadScoresForLeaderboard(LeaderboardID, GPGLeaderboardTimeScope.AllTime, false, aroundMyRankreuslts);
        Debug.LogWarning("call LoadScoresForLeaderboard");
#endif
    }

    public static string ShowingConcreteLeaderboardUI()
    {
        string ret = "ShowLeaderboardUI";
#if UNITY_IPHONE || UNITY_ANDROID
    // show leaderboard UI
         PlayGameServices.showLeaderboard(LeaderboardID, GPGLeaderboardTimeScope.AllTime);
#endif
        return ret;
    }



#if UNITY_IPHONE || UNITY_ANDROID

    private const string LeaderboardID = "CgkIuaqBk6sCEAIQAA";

    //private string text = "123";
	// Use this for initialization
	void Start ()
    {
        string s = MySocialEventListener.Instance.name;
	    Init();
	    Authenticate();
	    //Invoke("Authenticate",3f);
    }

    private void Authenticate()
    {
        PlayGameServices.setAchievementToastSettings(GPGToastPlacement.Center, 50);
        PlayGameServices.authenticate();
        Debug.LogWarning("MySocial authenticate");
    }


   

    public static string Init()
    {
        PlayGameServices.enableDebugLog(true);
#if UNITY_IPHONE 
    // we always want to call init as soon as possible after launch. Be sure to pass your own clientId to init on iOS!
    // This call is not required on Android.
        PlayGameServices.init("160040154367.apps.googleusercontent.com", true);//только для IOS.  Надо вставить clientID
#endif
        //GPGManager.loadScoresSucceededEvent += Instance.loadScoresSucceededEvent;

        return "inited";
    }


         public static GPGPlayerInfo GetLocalPlayerInfo()
     {
         
         var playerInfo = PlayGameServices.getLocalPlayerInfo();
         Prime31.Utils.logObject(playerInfo);

         // if we are on Android and have an avatar image available, lets download the profile pic
         //if (Application.platform == RuntimePlatform.Android && playerInfo.avatarUrl != null)
         //    PlayGameServices.loadProfileImageForUri(playerInfo.avatarUrl);
         return playerInfo;
     }
  

    //private static void EventsSubscibe()
    //{
        
    //    GPGManager.authenticationFailedEvent


    //}

#endif
}
